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

        public static void Welcome(string username){
            // array of nice messages
            string[] messages = new string[5];
            messages[0] = "Welcome to the Car Service";
            messages[1] = "We are delighted to have you among us.";
            messages[2] = "We hope you will enjoy your stay.";
            messages[3] = "We are happy to have you here.";
            messages[4] = "We are glad to have you here.";

            // random number generator
            Random rnd = new Random();
            int index = rnd.Next(0, 5);

            // show message
            MessageBox.Show(messages[index] + " " + username);

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
        }

        public static void Logout(Form form, int[] pos) {
            Button button = new Button();
            button.Text = "Logout";
            button.Location = new Point(pos[0], pos[1]);
            button.Size = new Size(100, 20);
            form.Controls.Add(button);

            button.Click += (sender, e) =>
            {
                form.Controls.Clear();
                Login(form);
            };
        }

        public static void Back(Form form, int[] pos, string username, string role) {
            Button backButton = new Button();
            backButton.Text = "Back";
            backButton.Location = new Point(10, 270);
            backButton.Size = new Size(100, 20);
            form.Controls.Add(backButton);

            backButton.Click += (sender, e) =>
            {
                form.Controls.Clear();
                UserPage(form, username, role);
            };
        }
        
        public static void SysAdminPage(Form form, string username) {
            // show a button which clears form and shows form of adding a new customer 

            form.Text = "SysAdmin Page";
            form.Size = new Size(400, 400);
            form.BackColor = Color.White;

            Welcome(username);

            // create a list of customers and buttons representing each CRUD operation

            Label customersLabel = new Label();
            customersLabel.Text = "Customers";
            customersLabel.Location = new Point(10, 10);
            customersLabel.Size = new Size(100, 20);
            form.Controls.Add(customersLabel);

            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                string query = "SELECT * FROM customers";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                // output all customers

                
                
                int i = 0;
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0), reader.GetString(1));
                    string CustomerID = reader.GetString(0);
                     
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
                        reader.Close();
                        UpdateCustomer(form, username, CustomerID, "sysadmin");
                        
                    };

                    Button DeleteButton = new Button();
                    DeleteButton.Text = "Delete";
                    DeleteButton.Location = new Point(210, 40 + i);
                    DeleteButton.Size = new Size(100, 20);
                    form.Controls.Add(DeleteButton);

                    DeleteButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        DeleteCustomer(form, username, CustomerID, "sysadmin");
                    };

                    i += 20;
                }

                reader.Close();
                
            }

            Button AddCustomerButton = new Button();
            AddCustomerButton.Text = "Add Customer";
            AddCustomerButton.Location = new Point(10, 110);
            AddCustomerButton.Size = new Size(100, 20);
            form.Controls.Add(AddCustomerButton);

            AddCustomerButton.Click += (sender, e) =>
            {
                form.Controls.Clear();
                AddCustomer(form, username, "sysadmin");
            };

            // users

            Label userLabel = new Label();
            userLabel.Text = "Users";
            userLabel.Location = new Point(10, 140);
            userLabel.Size = new Size(100, 20);
            form.Controls.Add(userLabel);

            if (DbCon.IsConnect())
            {
                string query = "SELECT * FROM staff";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    string userID = reader.GetString(0);

                    Label UserLabel = new Label();
                    UserLabel.Text = reader.GetString(1);
                    UserLabel.Location = new Point(10, 170 + i);
                    UserLabel.Size = new Size(100, 20);
                    form.Controls.Add(UserLabel);

                    Button UpdateButton = new Button();
                    UpdateButton.Text = "Update";
                    UpdateButton.Location = new Point(110, 170 + i);
                    UpdateButton.Size = new Size(100, 20);
                    form.Controls.Add(UpdateButton);

                    UpdateButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        UpdateUser(form, username, userID);

                    };

                    Button DeleteButton = new Button();
                    DeleteButton.Text = "Delete";
                    DeleteButton.Location = new Point(210, 170 + i);
                    DeleteButton.Size = new Size(100, 20);
                    form.Controls.Add(DeleteButton);

                    DeleteButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        DeleteUser(form, username, userID);
                    };

                    i += 20;
                }
                reader.Close();
            }

            Button AddUserButton = new Button();
            AddUserButton.Text = "Add User";
            AddUserButton.Location = new Point(10, 280);
            AddUserButton.Size = new Size(100, 20);
            form.Controls.Add(AddUserButton);

            AddUserButton.Click += (sender, e) =>
            {
                form.Controls.Clear();
                AddUser(form, username);
            };

            // jobs

            Label jobsLabel = new Label();
            jobsLabel.Text = "Jobs";
            jobsLabel.Location = new Point(10, 310);
            jobsLabel.Size = new Size(100, 20);
            form.Controls.Add(jobsLabel);

            if (DbCon.IsConnect())
            {
                string query = "SELECT * FROM jobs";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    string jobID = reader.GetString(0);

                    Label JobLabel = new Label();
                    JobLabel.Text = reader.GetString(1);
                    JobLabel.Location = new Point(10, 340 + i);
                    JobLabel.Size = new Size(100, 20);
                    form.Controls.Add(JobLabel);

                    Button UpdateButton = new Button();
                    UpdateButton.Text = "Update";
                    UpdateButton.Location = new Point(110, 340 + i);
                    UpdateButton.Size = new Size(100, 20);
                    form.Controls.Add(UpdateButton);

                    UpdateButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        UpdateJob(form, username, jobID, "sysadmin");

                    };

                    Button DeleteButton = new Button();
                    DeleteButton.Text = "Delete";
                    DeleteButton.Location = new Point(210, 340 + i);
                    DeleteButton.Size = new Size(100, 20);
                    form.Controls.Add(DeleteButton);

                    DeleteButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        DeleteJob(form, username, jobID, "sysadmin");
                    };

                    i += 20;
                }
                reader.Close();
            }

            Button AddJobButton = new Button();
            AddJobButton.Text = "Add Job";
            AddJobButton.Location = new Point(10, 480);
            AddJobButton.Size = new Size(100, 20);
            form.Controls.Add(AddJobButton);

            AddJobButton.Click += (sender, e) =>
            {
                form.Controls.Clear();
                AddJob(form, username, "sysadmin");
            };

            // tasks

            Label tasksLabel = new Label();
            tasksLabel.Text = "Tasks";
            tasksLabel.Location = new Point(10, 510);
            tasksLabel.Size = new Size(100, 20);
            form.Controls.Add(tasksLabel);

            if (DbCon.IsConnect())
            {
                string query = "SELECT * FROM tasks";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    string taskID = reader.GetString(0);

                    Label TaskLabel = new Label();
                    TaskLabel.Text = reader.GetString(1);
                    TaskLabel.Location = new Point(10, 540 + i);
                    TaskLabel.Size = new Size(100, 20);
                    form.Controls.Add(TaskLabel);

                    Button UpdateButton = new Button();
                    UpdateButton.Text = "Update";
                    UpdateButton.Location = new Point(110, 540 + i);
                    UpdateButton.Size = new Size(100, 20);
                    form.Controls.Add(UpdateButton);

                    UpdateButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        UpdateTask(form, username, taskID, "sysadmin");

                    };

                    Button DeleteButton = new Button();
                    DeleteButton.Text = "Delete";
                    DeleteButton.Location = new Point(210, 540 + i);
                    DeleteButton.Size = new Size(100, 20);
                    form.Controls.Add(DeleteButton);

                    DeleteButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        DeleteTask(form, username, taskID, "sysadmin");
                    };

                    i += 20;
                }
                reader.Close();
            }

            Logout(form, new int[] { 10, 900 });

        }

        public static void AddUser(Form form, string username) {
            Label userLabel = new Label();
            userLabel.Text = "Welcome " + username;
            userLabel.Location = new Point(10, 10);
            userLabel.Size = new Size(100, 20);
            form.Controls.Add(userLabel);

            Label nameLabel = new Label();
            nameLabel.Text = "Username";
            nameLabel.Location = new Point(10, 40);
            nameLabel.Size = new Size(100, 20);
            form.Controls.Add(nameLabel);

            TextBox name = new TextBox();
            name.Location = new Point(10, 60);
            name.Size = new Size(100, 20);
            form.Controls.Add(name);

            Label passwordLabel = new Label();
            passwordLabel.Text = "Password";
            passwordLabel.Location = new Point(10, 90);
            passwordLabel.Size = new Size(100, 20);
            form.Controls.Add(passwordLabel);

            TextBox password = new TextBox();
            password.Location = new Point(10, 110);
            password.Size = new Size(100, 20);
            form.Controls.Add(password);

            Label roleLabel = new Label();
            roleLabel.Text = "Role";
            roleLabel.Location = new Point(10, 140);
            roleLabel.Size = new Size(100, 20);
            form.Controls.Add(roleLabel);

            ComboBox role = new ComboBox();
            role.Items.Add("sysadmin");
            role.Items.Add("headmechanic");
            role.Items.Add("mechanic");
            role.Items.Add("offadmin");
            role.Location = new Point(10, 160);
            role.Size = new Size(100, 20);
            form.Controls.Add(role);

            Button button = new Button();
            button.Text = "Add User";
            button.Location = new Point(10, 190);
            button.Size = new Size(100, 20);
            form.Controls.Add(button);

            button.Click += (sender, e) =>
            {
                DBConnection dbCon = DBConfig();
                if (dbCon.IsConnect())
                {
                    string query = "INSERT INTO staff (username, password, role) VALUES ('" + name.Text + "', '" + password.Text + "', '" + role.Text + "')";
                    MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                    cmd.ExecuteNonQuery();
                    dbCon.Reset();
                }

                form.Controls.Clear();
                UserPage(form, username, "sysadmin");
            };

            // back button
            Back(form, new int[] { 10, 270 }, username, "sysadmin");

        }

        public static void UpdateUser(Form form, string username, string userID) {
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

            Label passwordLabel = new Label();
            passwordLabel.Text = "Password";
            passwordLabel.Location = new Point(10, 190);
            passwordLabel.Size = new Size(100, 20);
            form.Controls.Add(passwordLabel);

            TextBox password = new TextBox();
            password.Location = new Point(10, 210);
            password.Size = new Size(100, 20);
            form.Controls.Add(password);

            Label roleLabel = new Label();
            roleLabel.Text = "Role";
            roleLabel.Location = new Point(10, 240);
            roleLabel.Size = new Size(100, 20);
            form.Controls.Add(roleLabel);

            ComboBox role = new ComboBox();
            role.Items.Add("sysadmin");
            role.Items.Add("headmechanic");
            role.Items.Add("mechanic");
            role.Items.Add("offadmin");
            role.Location = new Point(10, 260);
            role.Size = new Size(100, 20);
            form.Controls.Add(role);

            Button button = new Button();
            button.Text = "Update User";
            button.Location = new Point(10, 240);
            button.Size = new Size(100, 20);
            form.Controls.Add(button);

            button.Click += (sender, e) =>
            {
                DBConnection dbCon = DBConfig();
                if (dbCon.IsConnect())
                {
                    string query = "UPDATE staff SET name = '" + name.Text + "', password = '" + password.Text + "', role = '" + role.Text + "' WHERE id = " + userID;
                    MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                    cmd.ExecuteNonQuery();
                    dbCon.Reset();
                }

                form.Controls.Clear();
                UserPage(form, username, "sysadmin");
            };

            // back button
            Back(form, new int[] { 10, 270 }, username, "sysadmin");
        }

        public static void DeleteUser(Form form, string username, string userID) {
            DBConnection dbCon = DBConfig();
            if (dbCon.IsConnect())
            {
                string query = "DELETE FROM staff WHERE id = " + userID;

                MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.ExecuteNonQuery();
                dbCon.Reset();
            }

            form.Controls.Clear();
            UserPage(form, username, "sysadmin");
        }


        public static void AddCustomer(Form form, string username, string role) {
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
                UserPage(form, username, role);
            };

            // back button
            Back(form, new int[] { 10, 270 }, username, role);
        }

        public static void UpdateCustomer(Form form, string username, string id, string role) {
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
                UserPage(form, username, role);
            };

            // back button
            Back(form, new int[] { 10, 270 }, username, role);

        }

        public static void DeleteCustomer(Form form, string username, string id, string role) {
            DBConnection dbCon = DBConfig();
            if (dbCon.IsConnect())
            {
                string query = "DELETE FROM customers WHERE id = " + id;

                MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.ExecuteNonQuery();
                dbCon.Reset();
            }

            form.Controls.Clear();
            UserPage(form, username, role);
        }


        public static void HeadMechanicPage(Form form, string username) {
            
            // maximum of 10 tasks shown at a time

            form.Text = "Head Mechanic Page";
            form.Size = new Size(400, 400);
            form.BackColor = Color.White;

            Welcome(username);

            Label jobsLabel = new Label();
            jobsLabel.Text = "Jobs";
            jobsLabel.Location = new Point(10, 10);
            jobsLabel.Size = new Size(100, 20);
            form.Controls.Add(jobsLabel);


            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                // maximum of 3 jobs shown at a time and taskstatus has the value "not started"
                string query = "SELECT * FROM jobs LIMIT 3";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    string id = reader.GetString(0);

                    Label jobDescription = new Label();
                    jobDescription.Text = reader.GetString("jobdescription");
                    jobDescription.Location = new Point(10, 30 + (i * 30));
                    jobDescription.Size = new Size(100, 20);
                    form.Controls.Add(jobDescription);

                    Button updateJob = new Button();
                    updateJob.Text = "Update Job";
                    updateJob.Location = new Point(110, 30 + (i * 30));
                    updateJob.Size = new Size(100, 20);
                    form.Controls.Add(updateJob);

                    updateJob.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        UpdateJob(form, username, id, "headmechanic");
                    };

                    Button deleteJob = new Button();
                    deleteJob.Text = "Delete Job";
                    deleteJob.Location = new Point(210, 30 + (i * 30));
                    deleteJob.Size = new Size(100, 20);
                    form.Controls.Add(deleteJob);

                    deleteJob.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        DeleteJob(form, username, id, "headmechanic");
                    };

                    i++;
                }
                reader.Close();
            }

            Label tasksLabel = new Label();
            tasksLabel.Text = "Tasks";
            tasksLabel.Location = new Point(10, 140);
            tasksLabel.Size = new Size(100, 20);
            form.Controls.Add(tasksLabel);

            if (DbCon.IsConnect()){
                // maximum of 10 tasks shown at a time and taskstatus has the value "not started"
                string query = "SELECT * FROM tasks LIMIT 10";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    string id = reader.GetString("id");

                    // show task description 
                    Label taskDescription = new Label();
                    taskDescription.Text = reader.GetString("taskdescription");
                    taskDescription.Location = new Point(10, 160 + (i * 30));
                    taskDescription.Size = new Size(100, 20);
                    form.Controls.Add(taskDescription);

                    Button updateTask = new Button();
                    updateTask.Text = "Update Task";
                    updateTask.Location = new Point(110, 160 + (i * 30));
                    updateTask.Size = new Size(100, 20);
                    form.Controls.Add(updateTask);

                    updateTask.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        UpdateTask(form, username, id, "headmechanic");
                    };

                    Button deleteTask = new Button();
                    deleteTask.Text = "Delete Task";
                    deleteTask.Location = new Point(210, 160 + (i * 30));
                    deleteTask.Size = new Size(100, 20);
                    form.Controls.Add(deleteTask);

                    deleteTask.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        DeleteTask(form, username, id, "headmechanic");

                    };

                    i++;
                }
                reader.Close();

            }

            // addJob button
            Button addJob = new Button();
            addJob.Text = "Add Job";
            addJob.Location = new Point(10, 600);
            addJob.Size = new Size(100, 20);
            form.Controls.Add(addJob);

            addJob.Click += (sender, e) =>
            {
                form.Controls.Clear();
                AddJob(form, username, "headmechanic");
            };

            // addTask button

            Button addTask = new Button();
            addTask.Text = "Add Task";
            addTask.Location = new Point(10, 630);
            addTask.Size = new Size(100, 20);
            form.Controls.Add(addTask);

            addTask.Click += (sender, e) =>
            {
                form.Controls.Clear();
                AddTask(form, username, "headmechanic");
            };

            // logout button

            Logout(form, new int[] { 10, 660 });

        }

        public static void AddJob(Form form, string username, string role)
        {

            // customer id 
            Label customerIdLabel = new Label();
            customerIdLabel.Text = "Customer ID";
            customerIdLabel.Location = new Point(10, 10);
            customerIdLabel.Size = new Size(100, 20);
            form.Controls.Add(customerIdLabel);

            ComboBox customerId = new ComboBox();
            // list all usenames from the customers table
            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                string query = "SELECT name FROM customers";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customerId.Items.Add(reader.GetString("name"));
                }
                reader.Close();
            }
            customerId.Location = new Point(10, 30);
            customerId.Size = new Size(100, 20);
            form.Controls.Add(customerId);

            // staff id

            Label staffIdLabel = new Label();
            staffIdLabel.Text = "Staff ID";
            staffIdLabel.Location = new Point(10, 60);
            staffIdLabel.Size = new Size(100, 20);
            form.Controls.Add(staffIdLabel);

            ComboBox staffId = new ComboBox();
            // list all usenames from the staff table
            if (DbCon.IsConnect())
            {
                string query = "SELECT id, username FROM staff";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                string id = reader.GetString(0);

                while (reader.Read())
                {
                    staffId.Items.Add(reader.GetString(1));
                }
                reader.Close();
            }
            staffId.Location = new Point(10, 80);
            staffId.Size = new Size(100, 20);
            form.Controls.Add(staffId);

            // job description
            Label jobDescriptionLabel = new Label();
            jobDescriptionLabel.Text = "Job Description";
            jobDescriptionLabel.Location = new Point(10, 60);
            jobDescriptionLabel.Size = new Size(100, 20);
            form.Controls.Add(jobDescriptionLabel);

            TextBox jobDescription = new TextBox();
            jobDescription.Location = new Point(10, 80);
            jobDescription.Size = new Size(100, 20);
            form.Controls.Add(jobDescription);

            // job status

            Label jobStatusLabel = new Label();
            jobStatusLabel.Text = "Job Status";
            jobStatusLabel.Location = new Point(10, 110);
            jobStatusLabel.Size = new Size(100, 20);
            form.Controls.Add(jobStatusLabel);

            ComboBox jobStatus = new ComboBox();
            jobStatus.Items.Add("not started");
            jobStatus.Items.Add("in progress");
            jobStatus.Items.Add("completed");
            jobStatus.Location = new Point(10, 130);
            jobStatus.Size = new Size(100, 20);
            form.Controls.Add(jobStatus);

            // job notes
            Label jobNotesLabel = new Label();
            jobNotesLabel.Text = "Job Notes";
            jobNotesLabel.Location = new Point(10, 160);
            jobNotesLabel.Size = new Size(100, 20);
            form.Controls.Add(jobNotesLabel);

            TextBox jobNotes = new TextBox();
            jobNotes.Location = new Point(10, 180);
            jobNotes.Size = new Size(100, 20);
            form.Controls.Add(jobNotes);

            // submit button
            Button submit = new Button();
            submit.Text = "Add Job";
            submit.Location = new Point(10, 210);
            submit.Size = new Size(100, 20);
            form.Controls.Add(submit);

            submit.Click += (sender, e) =>
            {
                // get the customer id from the username
                string customerIdValue = "";
                if (DbCon.IsConnect())
                {
                    string query = "SELECT id FROM customers WHERE name = '" + customerId.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        customerIdValue = reader.GetString("id");
                    }
                    reader.Close();
                }

                // insert the job into the database
                if (DbCon.IsConnect())
                {
                    string query = "INSERT INTO jobs (customer_id, staff_id, jobdescription, jobstatus, jobnotes) VALUES ('" + customerIdValue + "', '" + staffId.Text + "', '" + jobDescription.Text + "', '" + jobStatus.Text + "', '" + jobNotes.Text + "')";
                    MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                    cmd.ExecuteNonQuery();
                }

                form.Controls.Clear();
                UserPage(form, username, role);
            };

            // back button
            Back(form, new int[] { 10, 240 }, username, role);

        }

        public static void AddTask(Form form, string username, string role)
        {
            // job id 
            Label jobIdLabel = new Label();
            jobIdLabel.Text = "Job ID";
            jobIdLabel.Location = new Point(10, 10);
            jobIdLabel.Size = new Size(100, 20);
            form.Controls.Add(jobIdLabel);

            ComboBox jobId = new ComboBox();
            // list all usenames from the customers table
            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                string query = "SELECT id FROM jobs";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    jobId.Items.Add(reader.GetString("id"));
                }
                reader.Close();
            }
            jobId.Location = new Point(10, 30);
            jobId.Size = new Size(100, 20);
            form.Controls.Add(jobId);

            // task description
            Label taskDescriptionLabel = new Label();
            taskDescriptionLabel.Text = "Task Description";
            taskDescriptionLabel.Location = new Point(10, 60);
            taskDescriptionLabel.Size = new Size(100, 20);
            form.Controls.Add(taskDescriptionLabel);

            TextBox taskDescription = new TextBox();
            taskDescription.Location = new Point(10, 80);
            taskDescription.Size = new Size(100, 20);
            form.Controls.Add(taskDescription);

            // task status

            Label taskStatusLabel = new Label();
            taskStatusLabel.Text = "Task Status";
            taskStatusLabel.Location = new Point(10, 110);
            taskStatusLabel.Size = new Size(100, 20);
            form.Controls.Add(taskStatusLabel);

            ComboBox taskStatus = new ComboBox();
            taskStatus.Items.Add("not started");
            taskStatus.Items.Add("in progress");
            taskStatus.Items.Add("completed");
            taskStatus.Location = new Point(10, 130);
            taskStatus.Size = new Size(100, 20);
            form.Controls.Add(taskStatus);

            // task notes
            Label taskNotesLabel = new Label();
            taskNotesLabel.Text = "Task Notes";
            taskNotesLabel.Location = new Point(10, 160);
            taskNotesLabel.Size = new Size(100, 20);
            form.Controls.Add(taskNotesLabel);

            TextBox taskNotes = new TextBox();
            taskNotes.Location = new Point(10, 180);
            taskNotes.Size = new Size(100, 20);
            form.Controls.Add(taskNotes);

            // submit button
            Button submit = new Button();
            submit.Text = "Add Task";
            submit.Location = new Point(10, 210);
            submit.Size = new Size(100, 20);
            form.Controls.Add(submit);

            submit.Click += (sender, e) =>
            {
                // insert the task into the database
                if (DbCon.IsConnect())
                {
                    string query = "INSERT INTO tasks (jobid, taskdescription, taskstatus, tasknotes) VALUES ('" + jobId.Text + "', '" + taskDescription.Text + "', '" + taskStatus.Text + "', '" + taskNotes.Text + "')";
                    MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                    cmd.ExecuteNonQuery();
                }

                form.Controls.Clear();
                UserPage(form, username, role);
            };

            Back(form, new int[] { 10, 240 }, username, role);

        }

        public static void UpdateJob(Form form, string username, string id, string role)
        {
            

            // customer id
            Label customerIdLabel = new Label();
            customerIdLabel.Text = "Customer ID";
            customerIdLabel.Location = new Point(10, 10);
            customerIdLabel.Size = new Size(100, 20);
            form.Controls.Add(customerIdLabel);


            ComboBox customerId = new ComboBox();
            // list all usenames from the customers table
            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                string query = "SELECT name FROM customers";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customerId.Items.Add(reader.GetString("name"));
                }
                reader.Close();
            }
            customerId.Location = new Point(10, 30);
            customerId.Size = new Size(100, 20);
            form.Controls.Add(customerId);

            // staff id
            Label staffIdLabel = new Label();
            staffIdLabel.Text = "Staff ID";
            staffIdLabel.Location = new Point(10, 60);
            staffIdLabel.Size = new Size(100, 20);
            form.Controls.Add(staffIdLabel);

            ComboBox staffId = new ComboBox();
            // list all usenames from the staff table where role is mechanic
            if (DbCon.IsConnect())
            {
                string query = "SELECT username FROM staff WHERE role = 'mechanic'";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    staffId.Items.Add(reader.GetString("username"));
                }
                reader.Close();
            }

            staffId.Location = new Point(10, 80);
            staffId.Size = new Size(100, 20);
            form.Controls.Add(staffId);

            // job description
            Label jobDescriptionLabel = new Label();
            jobDescriptionLabel.Text = "Job Description";
            jobDescriptionLabel.Location = new Point(10, 110);
            jobDescriptionLabel.Size = new Size(100, 20);
            form.Controls.Add(jobDescriptionLabel);

            TextBox jobDescription = new TextBox();
            jobDescription.Location = new Point(10, 130);
            jobDescription.Size = new Size(100, 20);
            form.Controls.Add(jobDescription);

            // job status
            Label jobStatusLabel = new Label();
            jobStatusLabel.Text = "Job Status";
            jobStatusLabel.Location = new Point(10, 160);
            jobStatusLabel.Size = new Size(100, 20);
            form.Controls.Add(jobStatusLabel);

            ComboBox jobStatus = new ComboBox();
            jobStatus.Items.Add("not started");
            jobStatus.Items.Add("in progress");
            jobStatus.Items.Add("completed");
            jobStatus.Location = new Point(10, 180);
            jobStatus.Size = new Size(100, 20);
            form.Controls.Add(jobStatus);

            // job notes
            Label jobNotesLabel = new Label();
            jobNotesLabel.Text = "Job Notes";
            jobNotesLabel.Location = new Point(10, 210);
            jobNotesLabel.Size = new Size(100, 20);
            form.Controls.Add(jobNotesLabel);

            TextBox jobNotes = new TextBox();
            jobNotes.Location = new Point(10, 230);
            jobNotes.Size = new Size(100, 20);
            form.Controls.Add(jobNotes);

            // job cost

            Label jobCostLabel = new Label();
            jobCostLabel.Text = "Job Cost";
            jobCostLabel.Location = new Point(10, 260);
            jobCostLabel.Size = new Size(100, 20);
            form.Controls.Add(jobCostLabel);

            TextBox jobCost = new TextBox();
            jobCost.Location = new Point(10, 280);
            jobCost.Size = new Size(100, 20);
            form.Controls.Add(jobCost);

            // sign off button

            Button signOff = new Button();
            signOff.Text = "Sign Off";
            signOff.Location = new Point(10, 310);
            signOff.Size = new Size(100, 20);
            
            // signoff boolean can only be changed when all taskstatus associated with the job are completed

            signOff.Click += (sender, e) =>
            {
                // insert the job into the database
                if (DbCon.IsConnect())
                {
                    // TODO: check if all taskstatus associated with the job are completed if they are then signoff can be true
                    string query = "Select taskstatus FROM tasks WHERE jobid = '" + id + "'";

                    MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // check if all taskstatus are completed
                    while (reader.Read())
                    {
                        if (reader.GetString("taskstatus") != "completed")
                        {
                            MessageBox.Show("All tasks must be completed before signing off");
                            return;
                        }
                        else{
                            // allow signoff to be true
                            signOff.Enabled = true;
                            
                        }
                        

                    }
                    reader.Close();
                }
            };

            form.Controls.Add(signOff);

            // insert the job into the database
            Button submit = new Button();
            submit.Text = "Update Job";
            submit.Location = new Point(10, 340);
            submit.Size = new Size(100, 20);
            form.Controls.Add(submit);

            submit.Click += (sender, e) =>
            {
                if (DbCon.IsConnect())
                {
                    string query = "UPDATE jobs SET customer_id = '" + customerId.Text + "', staff_id = '" + staffId.Text + "', jobdescription = '" + jobDescription.Text + "', jobstatus = '" + jobStatus.Text + "', jobnotes = '" + jobNotes.Text + "', jobcost = '" + jobCost.Text + "', signoff = '" + signOff.Text + "' WHERE id = '" + id + "'";

                    MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Job Updated");
                }

                form.Controls.Clear();
                UserPage(form, username, role);
            };

            Back(form, new int[] { 10, 370 }, username, role);
        }

        public static void UpdateTask(Form form, string username, string id, string role) {
            Label jobIdLabel = new Label();
            jobIdLabel.Text = "Job ID";
            jobIdLabel.Location = new Point(10, 10);
            jobIdLabel.Size = new Size(100, 20);
            form.Controls.Add(jobIdLabel);

            ComboBox jobId = new ComboBox();
            // list all usenames from the customers table
            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                string query = "SELECT id FROM jobs";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    jobId.Items.Add(reader.GetString("id"));
                }
                reader.Close();
            }
            jobId.Location = new Point(10, 30);
            jobId.Size = new Size(100, 20);
            form.Controls.Add(jobId);

            // task description
            Label taskDescriptionLabel = new Label();
            taskDescriptionLabel.Text = "Task Description";
            taskDescriptionLabel.Location = new Point(10, 60);
            taskDescriptionLabel.Size = new Size(100, 20);
            form.Controls.Add(taskDescriptionLabel);

            TextBox taskDescription = new TextBox();
            taskDescription.Location = new Point(10, 80);
            taskDescription.Size = new Size(100, 20);
            form.Controls.Add(taskDescription);

            // task status

            Label taskStatusLabel = new Label();
            taskStatusLabel.Text = "Task Status";
            taskStatusLabel.Location = new Point(10, 110);
            taskStatusLabel.Size = new Size(100, 20);
            form.Controls.Add(taskStatusLabel);

            ComboBox taskStatus = new ComboBox();
            taskStatus.Items.Add("not started");
            taskStatus.Items.Add("in progress");
            taskStatus.Items.Add("completed");
            taskStatus.Location = new Point(10, 130);
            taskStatus.Size = new Size(100, 20);
            form.Controls.Add(taskStatus);

            // task notes
            Label taskNotesLabel = new Label();
            taskNotesLabel.Text = "Task Notes";
            taskNotesLabel.Location = new Point(10, 160);
            taskNotesLabel.Size = new Size(100, 20);
            form.Controls.Add(taskNotesLabel);

            TextBox taskNotes = new TextBox();
            taskNotes.Location = new Point(10, 180);
            taskNotes.Size = new Size(100, 20);
            form.Controls.Add(taskNotes);

            // insert the job into the database
            Button submit = new Button();
            submit.Text = "Update Task";
            submit.Location = new Point(10, 210);
            submit.Size = new Size(100, 20);
            form.Controls.Add(submit);

            submit.Click += (sender, e) =>
            {
                if (DbCon.IsConnect())
                {
                    string query = "UPDATE tasks SET jobid = '" + jobId.Text + "', taskdescription = '" + taskDescription.Text + "', taskstatus = '" + taskStatus.Text + "', tasknotes = '" + taskNotes.Text + "' WHERE id = '" + id + "'";

                    MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Task Updated");
                }

                form.Controls.Clear();
                UserPage(form, username, role);
            };

            Back(form, new int[] { 10, 240 }, username, role);
            
        }

        public static void DeleteJob(Form form, string username, string id, string role) {
            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                string query = "DELETE FROM jobs WHERE id = '" + id + "'";

                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                cmd.ExecuteNonQuery();
            }

            form.Controls.Clear();
            UserPage(form, username, role);
        }

        public static void DeleteTask(Form form, string username, string id, string role) {
            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                string query = "DELETE FROM tasks WHERE id = '" + id + "'";

                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                cmd.ExecuteNonQuery();
            }

            form.Controls.Clear();
            UserPage(form, username, role);
        }


        public static void MechanicPage(Form form, string username) {

            form.Text = "Mechanic Page";
            form.Size = new Size(400, 400);
            form.BackColor = Color.White;

            Welcome(username);

            Label jobsLabel = new Label();
            jobsLabel.Text = "Jobs";
            jobsLabel.Location = new Point(10, 10);
            jobsLabel.Size = new Size(100, 20);
            form.Controls.Add(jobsLabel);

            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                // maximum of 3 jobs shown at a time and taskstatus has the value "not started"
                string query = "SELECT * FROM jobs LIMIT 3";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    // show job description 
                    Label jobDescription = new Label();
                    jobDescription.Text = reader.GetString("jobdescription");
                    jobDescription.Location = new Point(10, 30 + (i * 30));
                    jobDescription.Size = new Size(100, 20);
                    form.Controls.Add(jobDescription);

                    i++;
                }
                reader.Close();
            }

            Label tasksLabel = new Label();
            tasksLabel.Text = "Tasks";
            tasksLabel.Location = new Point(10, 140);
            tasksLabel.Size = new Size(100, 20);
            form.Controls.Add(tasksLabel);

            if (DbCon.IsConnect())
            {
                // maximum of 3 tasks shown at a time and taskstatus has the value "not started"
                string query = "SELECT * FROM tasks LIMIT 10";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    string id = reader.GetString("id");

                    // show task description 
                    Label taskDescription = new Label();
                    taskDescription.Text = reader.GetString("taskdescription");
                    taskDescription.Location = new Point(10, 170 + (i * 30));
                    taskDescription.Size = new Size(100, 20);
                    form.Controls.Add(taskDescription);

                    Button updateTask = new Button();
                    updateTask.Text = "Update Task";
                    updateTask.Location = new Point(110, 170 + (i * 30));
                    updateTask.Size = new Size(100, 20);
                    form.Controls.Add(updateTask);

                    updateTask.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        UpdateTask(form, username, id, "mechanic");
                    };

                    i++;
                }
                reader.Close();

            }

            // logout
            Logout(form, new int[] { 10, 600 } );
            
        }

        public static void OffAdminPage(Form form, string username) {
            form.Text = "Office Administrator Page";
            form.Size = new Size(400, 400);
            form.BackColor = Color.White;

            Welcome(username);

            Label tasksLabel = new Label();
            tasksLabel.Text = "Jobs";
            tasksLabel.Location = new Point(10, 10);
            tasksLabel.Size = new Size(100, 20);
            form.Controls.Add(tasksLabel);

            DBConnection DbCon = DBConfig();
            if (DbCon.IsConnect())
            {
                // maximum of 3 jobs shown at a time and taskstatus has the value "not started"
                string query = "SELECT * FROM jobs LIMIT 3";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    string id = reader.GetString(0);

                    // show job description 
                    Label jobDescription = new Label();
                    jobDescription.Text = reader.GetString("jobdescription");
                    jobDescription.Location = new Point(10, 30 + (i * 30));
                    jobDescription.Size = new Size(100, 20);
                    form.Controls.Add(jobDescription);


                    Button updateJob = new Button();
                    updateJob.Text = "Update Job";
                    updateJob.Location = new Point(110, 30 + (i * 30));
                    updateJob.Size = new Size(100, 20);
                    form.Controls.Add(updateJob);

                    updateJob.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        UpdateJob(form, username, id, "offadmin");
                    };

                    Button deleteJob = new Button();
                    deleteJob.Text = "Delete Job";
                    deleteJob.Location = new Point(210, 30 + (i * 30));
                    deleteJob.Size = new Size(100, 20);
                    form.Controls.Add(deleteJob);

                    deleteJob.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        DeleteJob(form, username, id, "offadmin");
                    };

                    i++;
                }
                reader.Close();
            }

            Label jobsLabel = new Label();
            jobsLabel.Text = "Tasks";
            jobsLabel.Location = new Point(10, 140);
            jobsLabel.Size = new Size(100, 20);
            form.Controls.Add(jobsLabel);

            if (DbCon.IsConnect())
            {
                // maximum of 3 tasks shown at a time and taskstatus has the value "not started"
                string query = "SELECT * FROM tasks LIMIT 10";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    string id = reader.GetString("id");
                    // show task description 
                    Label taskDescription = new Label();
                    taskDescription.Text = reader.GetString("taskdescription");
                    taskDescription.Location = new Point(10, 170 + (i * 30));
                    taskDescription.Size = new Size(100, 20);
                    form.Controls.Add(taskDescription);

                    Button updateTask = new Button();
                    updateTask.Text = "Update Task";
                    updateTask.Location = new Point(110, 170 + (i * 30));
                    updateTask.Size = new Size(100, 20);
                    form.Controls.Add(updateTask);

                    updateTask.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        UpdateTask(form, username, id, "offadmin");
                    };

                    Button deleteTask = new Button();
                    deleteTask.Text = "Delete Task";
                    deleteTask.Location = new Point(210, 170 + (i * 30));
                    deleteTask.Size = new Size(100, 20);
                    form.Controls.Add(deleteTask);

                    deleteTask.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        DeleteTask(form, username, id, "offadmin");

                    };

                    i++;
                }
                reader.Close();

            }

            Label customersLabel = new Label();
            customersLabel.Text = "Customers";
            customersLabel.Location = new Point(10, 620);
            customersLabel.Size = new Size(100, 20);
            form.Controls.Add(customersLabel);

            if (DbCon.IsConnect())
            {
                string query = "SELECT * FROM customers";
                MySqlCommand cmd = new MySqlCommand(query, DbCon.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    string customerID = reader.GetString(0);

                    Label customerLabel = new Label();
                    customerLabel.Text = reader.GetString(1);
                    customerLabel.Location = new Point(10, 650 + i);
                    customerLabel.Size = new Size(100, 20);
                    form.Controls.Add(customerLabel);

                    Button UpdateButton = new Button();
                    UpdateButton.Text = "Update";
                    UpdateButton.Location = new Point(110, 650 + i);
                    UpdateButton.Size = new Size(100, 20);
                    form.Controls.Add(UpdateButton);

                    UpdateButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();
                        UpdateCustomer(form, username, customerID, "offadmin");

                    };

                    Button DeleteButton = new Button();
                    DeleteButton.Text = "Delete";
                    DeleteButton.Location = new Point(210, 650 + i);
                    DeleteButton.Size = new Size(100, 20);
                    form.Controls.Add(DeleteButton);

                    DeleteButton.Click += (sender, e) =>
                    {
                        form.Controls.Clear();
                        reader.Close();

                        DeleteCustomer(form, username, customerID, "offadmin");
                    };

                    i += 20;
                }
                reader.Close();

            }

            // addJob button
            Button addJob = new Button();
            addJob.Text = "Add Job";
            addJob.Location = new Point(10, 720);
            addJob.Size = new Size(100, 20);
            form.Controls.Add(addJob);

            addJob.Click += (sender, e) =>
            {
                form.Controls.Clear();
                AddJob(form, username, "offadmin");
            };

            // addTask button

            Button addTask = new Button();
            addTask.Text = "Add Task";
            addTask.Location = new Point(10, 740);
            addTask.Size = new Size(100, 20);
            form.Controls.Add(addTask);

            addTask.Click += (sender, e) =>
            {
                form.Controls.Clear();
                AddTask(form, username, "offadmin");
            };

            Logout(form, new int[] { 10, 770 });
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


                    for (int i = 0; i < 10; i++)
                    {
                        string query = "INSERT INTO tasks (job_id, taskdescription, taskstatus, tasknotes, taskdate) VALUES ('" + 1 + "', '" + "task description" + "', '" + "task status" + "', '" + "tasknotes" + "', '" + "taskdate" + "')";
                        MySqlCommand cmd = new MySqlCommand(query, dbCon.Connection);
                        cmd.ExecuteNonQuery();
                    }

                    dbCon.Reset();


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


// update columns names to a more readable format (camel case) 
// have it so that job and task your updating in realtime 

// show completed/closed in headmachanic page
// create new table for invoices and payments owed associated with customer