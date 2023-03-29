// See https://aka.ms/new-console-template for more information

using System;
using System.Windows.Forms;
using System.Drawing;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form();
            // DBQuery();
            Login(form);

            Application.Run(form);
            
        }

        public static void Login(Form form){
            form.Text = "Login Form";
            form.Size = new Size(400, 400);
            form.BackColor = Color.White;

            Label userLabel = new Label();
            userLabel.Text = "Username";
            userLabel.Location = new Point(10, 10);
            userLabel.Size = new Size(100, 20);
            form.Controls.Add(userLabel);

            TextBox userName = new TextBox();
            userName.Location = new Point(10, 30);
            userName.Size = new Size(100, 20);
            form.Controls.Add(userName);

            Label passLabel = new Label();
            passLabel.Text = "Password";
            passLabel.Location = new Point(10, 60);
            passLabel.Size = new Size(100, 20);
            form.Controls.Add(passLabel);

            TextBox passWord = new TextBox();
            passWord.Location = new Point(10, 80);
            passWord.Size = new Size(100, 20);
            form.Controls.Add(passWord);

            Button button = new Button();
            button.Text = "Login";
            button.Location = new Point(10, 110);
            button.Size = new Size(100, 20);
            form.Controls.Add(button);

            button.Click += (sender, e) =>
            {
                DBConnection dbCon = DBConfig();
                if (dbCon.IsConnect())
                {
                    string query = "SELECT * FROM staff WHERE username = '" + userName.Text + "' AND password = '" + passWord.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        form.Controls.Clear();
                        String username = reader.GetString(1);
                        String role = reader.GetString(3);
                        reader.Close();
                        UserPage(form, username, role);
                        dbCon.Reset();
                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password");
                        dbCon.Reset();
                    }
                }
            };
        }

        public static void UserPage(Form form, string username, string role) {
            // RBAC - Role Based Access Control

            if (role == "sysadmin") {
                SysAdminPage(form, username);
            }
            else if (role == "headmechanic") {
                HeadMechanicPage(form, username);
            }
            else if (role == "mechanic") {
                MechanicPage(form, username);
            }
            else if (role == "offadmin"){
                OffAdminPage(form, username);
            }

            Button button = new Button();
            button.Text = "Logout";
            button.Location = new Point(10, 110);
            button.Size = new Size(100, 20);
            form.Controls.Add(button);

            button.Click += (sender, e) =>
            {
                form.Controls.Clear();
                Login(form);
            };
        }
        
        public static void SysAdminPage(Form form, string username) {
            // show a button which clears form and shows form of adding a new customer 

            form.Text = "SysAdmin Page";
            form.Size = new Size(400, 400);
            form.BackColor = Color.White;

            // create a list of customers and buttons representing each CRUD operation

            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                string query = "SELECT * FROM customers";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                
                int i = 0;
                while (reader.Read())
                {
                    Label customerLabel = new Label();
                    customerLabel.Text = reader.GetString(1);
                    customerLabel.Location = new Point(10, 40 + i);
                    customerLabel.Size = new Size(100, 20);
                    form.Controls.Add(customerLabel);

                    Button UpdateButton = new Button();
                    UpdateButton.Text = "Update";
                    UpdateButton.Location = new Point(110, 40 + i);
                    UpdateButton.Size = new Size(100, 20);
                    form.Controls.Add(UpdateButton);

                    UpdateButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        UpdateCustomer(form, username, reader.GetString(0));
                        
                    };

                    Button DeleteButton = new Button();
                    DeleteButton.Text = "Delete";
                    DeleteButton.Location = new Point(210, 40 + i);
                    DeleteButton.Size = new Size(100, 20);
                    form.Controls.Add(DeleteButton);

                    DeleteButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        DeleteCustomer(form, username, reader.GetString(0));
                    };

                    i += 20;
                }

                reader.Close();
                
            }

            Button button = new Button();
            button.Text = "Add Customer";
            button.Location = new Point(10, 110);
            button.Size = new Size(100, 20);
            form.Controls.Add(button);

            button.Click += (sender, e) =>
            {
                form.Controls.Clear();
                AddCustomer(form, username);
            };

        }

        public static void AddCustomer(Form form, string username) {
            Label userLabel = new Label();
            userLabel.Text = "Welcome " + username;
            userLabel.Location = new Point(10, 10);
            userLabel.Size = new Size(100, 20);
            form.Controls.Add(userLabel);

            Label nameLabel = new Label();
            nameLabel.Text = "Name";
            nameLabel.Location = new Point(10, 40);
            nameLabel.Size = new Size(100, 20);
            form.Controls.Add(nameLabel);

            TextBox name = new TextBox();
            name.Location = new Point(10, 60);
            name.Size = new Size(100, 20);
            form.Controls.Add(name);

            Label phoneLabel = new Label();
            phoneLabel.Text = "Phone";
            phoneLabel.Location = new Point(10, 90);
            phoneLabel.Size = new Size(100, 20);
            form.Controls.Add(phoneLabel);

            TextBox phone = new TextBox();
            phone.Location = new Point(10, 110);
            phone.Size = new Size(100, 20);
            form.Controls.Add(phone);

            Label addressLabel = new Label();
            addressLabel.Text = "Address";
            addressLabel.Location = new Point(10, 140);
            addressLabel.Size = new Size(100, 20);
            form.Controls.Add(addressLabel);

            TextBox address = new TextBox();
            address.Location = new Point(10, 160);
            address.Size = new Size(100, 20);
            form.Controls.Add(address);

            Label emailLabel = new Label();
            emailLabel.Text = "Email";
            emailLabel.Location = new Point(10, 190);
            emailLabel.Size = new Size(100, 20);
            form.Controls.Add(emailLabel);

            TextBox email = new TextBox();
            email.Location = new Point(10, 210);
            email.Size = new Size(100, 20);
            form.Controls.Add(email);

            Button button = new Button();
            button.Text = "Add Customer";
            button.Location = new Point(10, 240);
            button.Size = new Size(100, 20);
            form.Controls.Add(button);

            button.Click += (sender, e) =>
            {
                DBConnection dbCon = DBConfig();
                if (dbCon.IsConnect())
                {
                    string query = "INSERT INTO customers (name, phone, address, email) VALUES ('" + name.Text + "', '" + phone.Text + "', '" + address.Text + "', '" + email.Text + "')";
                    MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                    cmd.ExecuteNonQuery();
                    dbCon.Reset();
                }

                form.Controls.Clear();
                UserPage(form, username, "sysadmin");
            };

        }

        public static void UpdateCustomer(Form form, string username, string id) {
            Label userLabel = new Label();
            userLabel.Text = "Welcome " + username;
            userLabel.Location = new Point(10, 10);
            userLabel.Size = new Size(100, 20);
            form.Controls.Add(userLabel);

            Label nameLabel = new Label();
            nameLabel.Text = "Name";
            nameLabel.Location = new Point(10, 40);
            nameLabel.Size = new Size(100, 20);
            form.Controls.Add(nameLabel);

            TextBox name = new TextBox();
            name.Location = new Point(10, 60);
            name.Size = new Size(100, 20);
            form.Controls.Add(name);

            Label phoneLabel = new Label();
            phoneLabel.Text = "Phone";
            phoneLabel.Location = new Point(10, 90);
            phoneLabel.Size = new Size(100, 20);
            form.Controls.Add(phoneLabel);

            TextBox phone = new TextBox();
            phone.Location = new Point(10, 110);
            phone.Size = new Size(100, 20);
            form.Controls.Add(phone);

            Label addressLabel = new Label();
            addressLabel.Text = "Address";
            addressLabel.Location = new Point(10, 140);
            addressLabel.Size = new Size(100, 20);
            form.Controls.Add(addressLabel);

            TextBox address = new TextBox();
            address.Location = new Point(10, 160);
            address.Size = new Size(100, 20);
            form.Controls.Add(address);

            Label emailLabel = new Label();
            emailLabel.Text = "Email";
            emailLabel.Location = new Point(10, 190);
            emailLabel.Size = new Size(100, 20);
            form.Controls.Add(emailLabel);

            TextBox email = new TextBox();
            email.Location = new Point(10, 210);
            email.Size = new Size(100, 20);
            form.Controls.Add(email);

            Button button = new Button();
            button.Text = "Update Customer";
            button.Location = new Point(10, 240);
            button.Size = new Size(100, 20);
            form.Controls.Add(button);

            button.Click += (sender, e) =>
            {
                DBConnection dbCon = DBConfig();
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE customers SET name = '" + name.Text + "', phone = '" + phone.Text + "', address = '" + address.Text + "', email = '" + email.Text + "' WHERE id = " + id;

                    MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                    cmd.ExecuteNonQuery();
                    dbCon.Reset();
                }

                form.Controls.Clear();
                UserPage(form, username, "sysadmin");
            };

        }

        public static void DeleteCustomer(Form form, string username, string id) {
            DBConnection dbCon = DBConfig();
            if (dbCon.IsConnect())
            {
                string query = "DELETE FROM customers WHERE id = " + id;

                MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.ExecuteNonQuery();
                dbCon.Reset();
            }

            form.Controls.Clear();
            UserPage(form, username, "sysadmin");
        }


        public static void HeadMechanicPage(Form form, string username) {
            // maximum of 3 jobs shown at a time
            // maximum of 10 tasks shown at a time
        }

        public static void MechanicPage(Form form, string username) {
            
        }

        public static void OffAdminPage(Form form, string username) {
            
        }

        // create a database connection
        public static DBConnection DBConfig() 
        {
            var dbCon = DBConnection.Instance();
            dbCon.Server = "localhost";
            dbCon.DatabaseName = "test";
            dbCon.UserName = "root";
            dbCon.Password = "123456";

            return dbCon;
        }

        public static void DBQuery()
        {
            var dbCon = DBConfig();
            if (dbCon.IsConnect())
            {
                if (dbCon.IsConnect())
                {
                    // any query you want to test run here. 

                    // TODO: staff should only have a choice of only 4 roles (sysadmin, headmechanic, mechanic, offadmin)


                    string query = "SELECT * FROM staff";
                    MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0) + " " + reader.GetString(1));
                    }
                    dbCon.Close();
                    
                }
            }
        }
    }

    public class DBConnection
    {
        private DBConnection() { 

        }

        public string Server { get; set; }
        public string DatabaseName { get; set; } 
        public string UserName { get; set; } 
        public string Password { get; set; }

        public MySqlConnection Connection { get; set; }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
            }

            return true;
        }

        public void Close()
        {
            Connection.Close();
        }

        // reset the connection
        public void Reset()
        {
            Connection = null;
        }
    }
}