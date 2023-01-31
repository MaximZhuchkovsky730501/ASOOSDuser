using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Globalization;
using System.IO;

namespace ASOOSDuser
{

    public partial class ASOOSDuser : Form
    {
        //////////////////// ЛОГИЧЕСКАЯ ЧАСТЬ ////////////////////// 

        private Settings settings;
        private List<Server> server_list;
        private Server server_gpk;
        List<User> users;

        private List<Server> init_server_list(List<String> lines)
        {
            List<Server> server = new List<Server>(); //создаём список серверов
            foreach (String line in lines)
            {
                String[] words = line.Split('/'); //разделяем строку на подстроки по символу "/" (содержимое строки: "имя подразделения/ip-адрес для подключения/servise name базы/department_id")
                server.Add(new Server(words[0], words[1], words[2], words[3])); //создаём экземпляры класса Server и добавляем их в список
            }
            //output.count = server.Count;
            return server; // возвращаем список серверов
        }

        private Server init_server_list(String line)
        {
            String[] words = line.Split('/'); //разделяем строку на подстроки по символу "/" (содержимое строки: "имя подразделения/ip-адрес для подключения/servise name базы/department_id")
            Server server = new Server(words[0], words[1], words[2], words[3]); //создаём экземпляры класса Server
            //output.count = server.Count;
            return server; // возвращаем список серверов
        }

        private String init_search_user_script(Dictionary<String, String> condition) //возвращает скрипт в ввиде списка команд
        {
            String command = "SELECT * FROM SCHEMA_NAME.USER_INFO WHERE (";
            foreach (var cnd in condition)
            {
                if (cnd.Equals(condition.Last()))
                    command = command + cnd.Key + " LIKE '" + cnd.Value + "%' OR " + cnd.Key + " LIKE '%" + cnd.Value.ToLower() + "%' OR " + cnd.Key + " LIKE '%" + cnd.Value.ToUpper() + "%')";
                else
                    command = command + cnd.Key + " LIKE '" + cnd.Value + "%' OR " + cnd.Key + " LIKE '%" + cnd.Value.ToLower() + "%' OR " + cnd.Key + " LIKE '%" + cnd.Value.ToUpper() + "%') AND (";
            }
            return command;
        }

        private String init_search_roles_script(String user_id) //возвращает скрипт в ввиде списка команд
        {
            String command = "SELECT ROLE_ID FROM SCHEMA_NAME.USER_ROLES WHERE USER_ID = '" + user_id + "'";
            return command;
        }

        private String init_search_roles_names_script(String role_id) //возвращает скрипт в ввиде списка команд
        {
            String command = "SELECT ROLE_NAME FROM SCHEMA_NAME.ROLES WHERE ROLE_ID = '" + role_id + "'";
            return command;
        }

        private Dictionary<String, String> roles_name_script_run(Server server, String login, String password, User user)
        {
            //List<Information> info_list = new List<Information>(); //список параметров, полученных из БД АСООСД
            OracleConnection con = new OracleConnection();  //текущее подключение                       \
            OracleCommand cmd = new OracleCommand();        //исполняемая команда                        > переменные необходимые для подключения к БД
            String connectionString;                        //стока для открытия БД (как в tnsnames)    /
            OracleDataReader dr;                            //объект для чтения пезультатов            /
            String roles_names_script;
            Dictionary<String, String> roles_names = new Dictionary<String,String>();
            try
            {
                connectionString = "Data Source = (DESCRIPTION = " +                                                            //
                                                   "(ADDRESS = (PROTOCOL = TCP)(HOST =  " + server.ip + ")(PORT = 8888)) " +    //
                                                   "(CONNECT_DATA = " +                                                         //инициализация строки подклюсения
                                                     "(SERVICE_NAME =  " + server.service_name + ") " +                         //
                                                   ") " +                                                                       //
                                                   ");User Id = " + login + ";password=" + password;                            //
                con.ConnectionString = connectionString;
                cmd.Connection = con;
                con.Open(); //подключется к БД с использованием ранее заданных свойств
                /////////////////////  поиск ролей пользователя /////////////////
                foreach (String role_id in user.roles)
                {
                    roles_names_script = init_search_roles_names_script(role_id);
                    cmd.CommandText = roles_names_script; //определяем команду для выполнения на сервере 
                    dr = cmd.ExecuteReader(0); //выполняем команду
                    while (dr.Read()) //пока результат выполнения скрипта успешно считывается
                    {
                        object[] res = new object[1]; //создаём новый массив ролей из 1-го элемнта, т.к. скрипт считывает только один столбец
                        dr.GetValues(res); //помещаем считанные данные в переменную
                        roles_names.Add(role_id, res[0].ToString());//добавляем роли пользоваетеля в словарь
                    }
                    dr.Close(); //закрываем считыватель результатов выполнения скрипта
                } //теперь каждому пользователю в списке соответствует свой список ролей
                con.Close(); //закрываем соединение
            }
            catch (OracleException e)
            {
                MessageBox.Show("При подключении к " + server.name + "возникла ошибка. Проверьте параметры подключения в файле настроек\n" + e.ToString(),
                    "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return roles_names; //возвращем список с информацией о параметрах
        }

        private List<User> search_script_run(Server server, String login, String password, Dictionary<String, String> condition)
        {
            //List<Information> info_list = new List<Information>(); //список параметров, полученных из БД АСООСД
            OracleConnection con = new OracleConnection();  //текущее подключение                       \
            OracleCommand cmd = new OracleCommand();        //исполняемая команда                        > переменные необходимые для подключения к БД
            String connectionString;                        //стока для открытия БД (как в tnsnames)    /
            OracleDataReader dr;                            //объект для чтения пезультатов            /
            String search_user_script; //скрипт для нахождения пользователей 
            String search_roles_script; //скрипт для нахождения ролей пользователя
            List<object[]> results_users = new List<object[]>(); //список данных о пользователях
            List<List<object[]>> results_user_roles = new List<List<object[]>>(); //список списков ролей пользователей
            List<User> users = new List<User>(); //список пользователей
            search_user_script = init_search_user_script(condition); //получаем список команд
            Console.WriteLine("получение данных...");
            try
            {
                connectionString = "Data Source = (DESCRIPTION = " +                                                            //
                                                   "(ADDRESS = (PROTOCOL = TCP)(HOST =  " + server.ip + ")(PORT = 8888)) " +    //
                                                   "(CONNECT_DATA = " +                                                         //инициализация строки подклюсения
                                                     "(SERVICE_NAME =  " + server.service_name + ") " +                         //
                                                   ") " +                                                                       //
                                                   ");User Id = " + login + ";password=" + password;                            //
                con.ConnectionString = connectionString;
                cmd.Connection = con;
                con.Open(); //подключется к БД с использованием ранее заданных свойств
                /////////////////////  поиск польхзователей /////////////////
                cmd.CommandText = search_user_script; //определяем команду для выполнения на сервере 
                dr = cmd.ExecuteReader(0); //выполняем команду
                while (dr.Read()) //пока результат выполнения скрипта успешно считывается
                {
                    object[] res = new object[16]; //временная переменная для считывания данных (16 - кол-во считаных столбцов)
                    dr.GetValues(res); //помещаем считанные данные в переменную
                    results_users.Add(res); //добавляем данные о пользоваетеле в список
                }
                /////////////////// поиск ролей //////////////////////////////
                foreach (object[] user in results_users) //для каждого элемента из списка данных о пользователе
                {
                    search_roles_script = init_search_roles_script(user[0].ToString()); //получаем список команд для нахождения ролей конкретного пользователя по его id (user[0] содержет USER_ID)
                    cmd.CommandText = search_roles_script; //определяем команду для выполнения на сервере 
                    dr = cmd.ExecuteReader(0); //выполняем команду
                    List<object[]> results_roles = new List<object[]>(); //создаём новый список ролей 
                    while (dr.Read()) //пока результат выполнения скрипта успешно считывается
                    {
                        object[] res = new object[1]; //создаём новый массив ролей из 1-го элемнта, т.к. скрипт считывает только один столбец
                        dr.GetValues(res); //помещаем считанные данные в переменную
                        results_roles.Add(res); //добавляем роли пользоваетеля в список
                    }
                    results_user_roles.Add(results_roles);//добавляем список роле пользоваетеля в другой список (для соостветсвия списков ролей и списка пользователей)
                } //теперь каждому пользователю в списке соответствует свой список ролей
                dr.Close(); //закрываем считыватель результатов выполнения скрипта
                //////////////// создание пользователей //////////////////////////
                foreach (object[] user in results_users) //для каждого элемента из списка данных о пользователе
                {
                    users.Add(new User(user)); //создаём пользователя
                    foreach (object[] role in results_user_roles.ElementAt(results_users.IndexOf(user))) //для каждого значения из списка ролей из элемента списка списков ролей соответствующего текущему пользователю
                    {
                        users.Last().set_role(role); //добавляем роль текущему пользователю
                    }
                }
                con.Close(); //закрываем соединение
            }
            catch (OracleException e)
            {
                MessageBox.Show("При подключении к " + server.name + "возникла ошибка. Проверьте параметры подключения в файле настроек\n" + e.ToString(),
                    "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return users; //возвращем список с информацией о параметрах
        }

        private List<String> init_user_replication_script(User user) //возвращает скрипт в ввиде списка команд
        {
            List<String> command = new List<string>();
            //command.Add("SET DEFINE OFF");
            command.Add("MERGE INTO SCHEMA_NAME.A_USER A USING " +
                        " (SELECT     " +
                        "  '" + user.data[0] + "' as USER_ID, " +
                        "  '" + user.data[1] + "' as USER_SURNAME, " +
                        "  '" + user.data[2] + "' as USER_FIRSTNAME, " +
                        "  '" + user.data[3] + "' as USER_LASTNAME, " +
                        "  '" + user.data[4] + "' as USER_PASSWORD, " +
                        "  " + user.data[5] + " as USER_PROPERTY, " +
                        "  '" + user.data[6] + "' as PWD_HASH, " +
                        "  " + user.data[7] + " as ORG_ID, " +
                        "  " + user.data[8] + " as ORG_ID_SUB, " +
                        "  " + user.data[9] + " as DEPARTMENT_ID, " +
                        "  " + user.data[10] + " as LEVEL_ORG, " +
                        "  " + user.data[11] + " as TYPE_OPK, " +
                        "  TO_DATE('" + Convert.ToDateTime(user.data[12]).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture) + "', 'MM/DD/YYYY HH24:MI:SS') as DATE_PASSWORD, " +
                        "  " + user.data[13] + " as FORCED_PASS_CHANGE, " +
                        "  " + user.data[14] + " as DIRECT_USER, " +
                        "  '" + user.data[15] + "' as USER_NAME " +
                        "  FROM DUAL) B " +
                        "ON (A.USER_ID = B.USER_ID) " +
                        "WHEN NOT MATCHED THEN  " +
                        "INSERT ( " +
                        "  USER_ID, USER_SURNAME, USER_FIRSTNAME, USER_LASTNAME, USER_PASSWORD,  " +
                        "  USER_PROPERTY, PWD_HASH, ORG_ID, ORG_ID_SUB, DEPARTMENT_ID,  " +
                        "  LEVEL_ORG, TYPE_OPK, DATE_PASSWORD, FORCED_PASS_CHANGE, DIRECT_USER,  " +
                        "  USER_NAME) " +
                        "VALUES ( " +
                        "  B.USER_ID, B.USER_SURNAME, B.USER_FIRSTNAME, B.USER_LASTNAME, B.USER_PASSWORD,  " +
                        "  B.USER_PROPERTY, B.PWD_HASH, B.ORG_ID, B.ORG_ID_SUB, B.DEPARTMENT_ID,  " +
                        "  B.LEVEL_ORG, B.TYPE_OPK, B.DATE_PASSWORD, B.FORCED_PASS_CHANGE, B.DIRECT_USER,  " +
                        "  B.USER_NAME) " +
                        "WHEN MATCHED THEN " +
                        "UPDATE SET  " +
                        "  A.USER_SURNAME = B.USER_SURNAME, " +
                        "  A.USER_FIRSTNAME = B.USER_FIRSTNAME, " +
                        "  A.USER_LASTNAME = B.USER_LASTNAME, " +
                        "  A.USER_PASSWORD = B.USER_PASSWORD, " +
                        "  A.USER_PROPERTY = B.USER_PROPERTY, " +
                        "  A.PWD_HASH = B.PWD_HASH, " +
                        "  A.ORG_ID = B.ORG_ID, " +
                        "  A.ORG_ID_SUB = B.ORG_ID_SUB, " +
                        "  A.DEPARTMENT_ID = B.DEPARTMENT_ID, " +
                        "  A.LEVEL_ORG = B.LEVEL_ORG, " +
                        "  A.TYPE_OPK = B.TYPE_OPK, " +
                        "  A.DATE_PASSWORD = B.DATE_PASSWORD, " +
                        "  A.FORCED_PASS_CHANGE = B.FORCED_PASS_CHANGE, " +
                        "  A.DIRECT_USER = B.DIRECT_USER, " +
                        "  A.USER_NAME = B.USER_NAME ");
            command.Add("COMMIT ");

            return command;
        }

        private List<String> init_role_replication_script(User user) //возвращает скрипт в ввиде списка команд
        {
            List<String> command = new List<string>();
            //command.Add("SET DEFINE OFF");
            command.Add("DELETE FROM SCHEMA_NAME.USER_ROLES WHERE USER_ID = '" + user.data[0] + "'");
            command.Add("COMMIT ");
            foreach (String role in user.roles) //для каждой роли добавляем свою команду
            {
                command.Add("Insert into SCHEMA_NAME.USER_ROLES " +
                               "(ROLE_ID, USER_ID) " +
                             "Values " +
                               "(" + role + ", '" + user.data[0] + "')");
            }
            command.Add("COMMIT ");
            return command;
        }

        private void replication_script_run(List<Server> servers, String login, String password, User user)
        {
            List<String> script = init_user_replication_script(user); //получаем скрипт для переноса пользователя
            foreach (String command in init_role_replication_script(user)) //получаем скрипт для переноса роей 
            {
                script.Add(command); //объеденяем два скрипта
            }
            Server server = servers.Find(item => item.department_id == user.data[9]); //определяем сервер для переноса
            OracleConnection con = new OracleConnection();  //текущее подключение                       \
            OracleCommand cmd = new OracleCommand();        //исполняемая команда                        > переменные необходимые для подключения к БД
            String connectionString;                        //стока для открытия БД (как в tnsnames)    /
            OracleDataReader dr;                            //объект для чтения пезультатов            /
            try
            {
                connectionString = "Data Source = (DESCRIPTION = " +                                                            //
                                                   "(ADDRESS = (PROTOCOL = TCP)(HOST =  " + server.ip + ")(PORT = 8888)) " +    //
                                                   "(CONNECT_DATA = " +                                                         //инициализация строки подклюсения
                                                     "(SERVICE_NAME =  " + server.service_name + ") " +                         //
                                                   ") " +                                                                       //
                                                   ");User Id = " + login + ";password=" + password;                            //
                con.ConnectionString = connectionString;
                cmd.Connection = con;
                con.Open(); //подключется к БД с использованием ранее заданных свойств
                foreach (String command in script)
                {
                    try
                    {
                        cmd.CommandText = command; //определяем команду для выполнения на сервере 
                        int num = cmd.ExecuteNonQuery(); //выполняем команду
                    }
                    catch (OracleException e) //возникает ошибка ora-00001: unique constraint violated
                    {                         //нарушение уникальности поля
                       // continue;             //игнорируем ошибку
                        MessageBox.Show(e.ToString(), "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                //dr.Close(); //закрываем считыватель результатов выполнения скрипта

                con.Close(); //закрываем соединение
            }
            catch (OracleException e)
            {
                throw new Exception("При подключении к " + server.name + " возникла ошибка. Проверьте параметры подключения в файле настроек\n" + e.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //////////////////// ВИЗУАЛЬНАЯ ЧАСТЬ ////////////////////// 

        public ASOOSDuser()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (tb_login.Enabled)
                tb_login.Enabled = false;
            else
                tb_login.Enabled = true;
        }

        private void init_cb_department(List<Server> server_list)
        {
            List<String> server_names = new List<String>();
            foreach (Server item in server_list)
            {
                server_names.Add(item.name);
            }
            cb_department.DataSource = server_names;
        }

        private void init_dgv_results(List<User> users)
        {
            dgv_result.RowCount = users.Count;
            foreach (User user in users)
            {
                String department = "";
                try
                {
                    if (user.data[9] == "-1")
                        department = "ГПК";
                    else
                        department = server_list.Find(item => item.department_id == user.data[9]).name;
                }
                catch
                {
                    department = "ID аодразделения = " + user.data[9];
                }
                String[] row = {user.data[3], user.data[2], user.data[1], user.data[15], department};
                dgv_result.Rows[users.IndexOf(user)].SetValues(row);
                if (user.data[5] == "1")
                {
                    dgv_result.Rows[users.IndexOf(user)].DefaultCellStyle.BackColor = Color.Red;
                }
            }
            dgv_result.Sort(dgv_result.Columns[0], ListSortDirection.Ascending);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            settings = new Settings(); //получаем основные параметры
            server_list = new List<Server>(); //создаём список серверов
            server_list = init_server_list(settings.get_server_lines()); //заполняем список серверов
            server_gpk = init_server_list(settings.get_server_gpk()); //отдельно определяем сервер для поиска пользователей
            init_cb_department(server_list);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            dgv_result.Rows.Clear();
            dgv_result.RowCount = 0;
            Dictionary<String, String> condition = new Dictionary<String, String>();
            String key = "";
            String value = "";
            try
            {
                if (chb_last_name.Checked)
                {
                    key = "LASTNAME";
                    if (String.IsNullOrWhiteSpace(tb_last_name.Text )) throw new Exception("Фамилия пользователя не указана");
                    value = tb_last_name.Text.ToLower();
                    value = Char.ToUpper(value[0]) + value.Substring(1);
                    condition.Add(key, value);
                }
                if (chb_login.Checked)
                {
                    key = "NAME";
                    if (String.IsNullOrWhiteSpace(tb_login.Text)) throw new Exception("Логин пользователя не указан");
                    value = tb_login.Text.ToUpper();
                    condition.Add(key, value);
                }
                if (chb_department.Checked)
                {
                    key = "ORGANISATION";
                    value = server_list.Find(item => item.name == cb_department.SelectedItem.ToString()).department_id;
                    condition.Add(key, value);
                }
                if (condition.Count == 0) throw new Exception("Параметры поиска не заданы");
                users = search_script_run(server_gpk, settings.get_login_gpk(), settings.get_password_gpk(), condition);
                init_dgv_results(users);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (dgv_result.CurrentRow != null)
            {
                btn_info.Enabled = true;
                btn_replic.Enabled = true;
                btn_role.Enabled = true;
            }
            else
            {

                btn_info.Enabled = false;
                btn_replic.Enabled = false;
                btn_role.Enabled = false;
            }
        }

        private void btn_replic_Click(object sender, EventArgs e)
        {
            List<User> selected_user = new List<User>();
            int count = 0;
            //for (int i = 0; i < count; i++)
            //{
            //    dgv_result.SelectedRows[i]
            //}
            foreach (DataGridViewRow row in dgv_result.SelectedRows)
            {
                try
                {
                    selected_user.Add(users.Find(item => item.data[15] == row.Cells[3].Value.ToString()));
                    if (selected_user == null) throw new Exception("Пользователь не выбран");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось определить выбранных пользователей:\n" + ex.Message, "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            foreach (User user in selected_user)
            {
                try
                {
                    Server server = null;
                    server = server_list.Find(item => item.department_id == user.data[9]); //определяем сервер для переноса
                    if (server == null) throw new Exception("Не удалось определить подразделение");
                        replication_script_run(server_list, settings.get_login_local(), settings.get_password_local(), user);
                        count++;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не перенести пользователя " + user.data[3] + ex.Message, "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }
            }
            
            MessageBox.Show("Перенесено " + count.ToString() + " пользователей из " + selected_user.Count.ToString(), "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            try
            {
                User selected_user = users.Find(item => item.data[15] == dgv_result.CurrentRow.Cells[3].Value.ToString());
                if (selected_user == null) throw new Exception("Пользователь не выбран");
                InfoForm info_form = new InfoForm(selected_user);
                info_form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btn_role_Click(object sender, EventArgs e)
        {
            try
            {
                User selected_user = users.Find(item => item.data[15] == dgv_result.CurrentRow.Cells[3].Value.ToString());
                if (selected_user == null) throw new Exception("Пользователь не выбран");
                RoleForm role_form = new RoleForm(roles_name_script_run(server_gpk, settings.get_login_gpk(), settings.get_password_gpk(), selected_user));
                role_form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void chb_login_CheckedChanged(object sender, EventArgs e)
        {
            tb_login.Enabled = !tb_login.Enabled;
        }

        private void chb_last_name_CheckedChanged(object sender, EventArgs e)
        {
            tb_last_name.Enabled = !tb_last_name.Enabled;
        }

        private void chb_department_CheckedChanged(object sender, EventArgs e)
        {
            cb_department.Enabled = !cb_department.Enabled;
        }

        
    }
    
    class Settings
    {
        private String login_gpk, password_gpk, login_local, password_local, server_gpk_line;
        private List<String> server_lines;

        public Settings()
        {
            try // конструкций для обработки исключений try - catch
            {
                String text = read_settings(); //чтение данных из файла
                int ind = text.IndexOf("begin") + 7;
                text = text.Substring(ind); //удаляем слово "begin", и  весь текст до него
                String[] lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries); // разбиваем текст по строкам
                login_gpk = lines[0].Split('/')[0]; // получаем логин
                password_gpk = lines[0].Split('/')[1]; // получаем пароль
                server_gpk_line = lines[1];
                login_local = lines[2].Split('/')[0]; // получаем логин
                password_local = lines[2].Split('/')[1]; // получаем пароль
                List<String> tmp = new List<String>(lines);
                tmp.RemoveRange(0, 3);
                lines = tmp.ToArray();
                server_lines = new List<string>(); // создаём список с информацией о серверах
                foreach (String line in lines)
                    server_lines.Add(line); //добовляем строки с информацией о серверах в список 
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка интерпритации данных из файла настроек, возможно данные записаны с ошибками или в другой последовательности\n" + e.ToString(),
                    "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Process.GetCurrentProcess().Kill(); // завершение работы приложения
            }
        }

        public String get_login_gpk()
        {
            return login_gpk;
        }

        public String get_password_gpk()
        {
            return password_gpk;
        }

        public String get_login_local()
        {
            return login_local;
        }

        public String get_password_local()
        {
            return password_local;
        }

        public List<String> get_server_lines()
        {
            return server_lines;
        }

        public String get_server_gpk()
        {
            return server_gpk_line;
        }

        static private String read_settings()
        {
            String file_text = null;
            try
            {
                FileStream file = File.OpenRead("settings.ini"); // открываем файл "settings.txt" в корневой папке приложения
                byte[] array = new byte[file.Length];
                int count = file.Read(array, 0, array.Length); // считываем байты из файла в массив "array", count - колличество успешно считанных байт
                file_text = System.Text.Encoding.UTF8.GetString(array); //декодируем байты в текст
                file.Close(); // закрываем файл
            }
            catch (DirectoryNotFoundException e)
            {
                MessageBox.Show("не удалось найти путь к файлу \"settings.txt\"\n" + e.ToString(),
                   "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("файл \"settings.txt\" не найден\n" + e.ToString(),
                   "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (FileLoadException e)
            {
                MessageBox.Show("не удалось загрузить файл \"settings.txt\"\n" + e.ToString(),
                   "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            catch (Exception e)
            {
                MessageBox.Show("неизвестная ошибка при чтении файла \"settings.txt\"\n" + e.ToString(),
                   "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            return file_text;
        }
    }

    class Server
    {
        public String name, ip, service_name, department_id;
        public Server(String n, String i, String s, String d)
        {
            name = n; //имя подразделения
            ip = i; //ip-адрес для подключения
            service_name = s; //servise name базы
            department_id = d; //department_id подразделения
        }
    }

    public class User
    {
        public String[] data;
        public List<String> roles;

        public User(object[] obj)
        {
            data = new String[16];
            roles = new List<string>();
            for (int i = 0; i < 16; i++)
            {
                data[i] = obj[i].ToString();
            }
        }

        public void set_role(object[] obj)
        {
            roles.Add(obj[0].ToString());
        }

        public static Dictionary<int, String> columns = new Dictionary<int, String>() //словарь соответствия 
        {
            {0, "ID"}, 
            {1, "LOGIN"},
            {2, "SURNAME"},
            {3, "FIRSTNAME"},
            {4, "LASTNAME"},
            {5, "DIRECTION_USER"},
            {6, "PROPERTY"},
            {7, "PASSWORD"},
            {8, "ORGANISATION_ID"},
            {9, "ORGORGANISATION_ID_SUBJECT"},
            {10, "ORGANISATION"},
            {11, "LEVEL_ORGANISATION"},
            {12, "TYPE_ORGANISATION"},
            {13, "LAST_CHANGE_PASSWORD_DATE"},
            {14, "FORCED_PASSWORD_CHANGE"},
            {15, "PASSWORD"},
        };
    }

}
