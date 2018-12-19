using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace MoviesRentalSystems
{
    public partial class Form1 : Form
    {
        public SqlConnection conn = null;
       public  bool Connection,Show_Lists;

        public Form1()
        {
            InitializeComponent();
           
            string connect = "Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;";  
            try {
                Connection = false;                                                                  
                conn = new SqlConnection(connect);                      // To Initiliazing the connection to The server.
                conn.Open();
                Connection = true;                                     // Connection is Available.
                SqlCommand cmd;
                Show_Lists = false;                                    
                // SQL Query for retriving data from the Database.
                String Sql_Movies = "Select MovieID,Title,Genre,Rental_Cost,Year From Movies";
                String Sql_Rentals = "Select RMID, C.FirstName as FirstName, C.LastName as LastName, C.Address as Address,M.Title as Title, M.Rental_Cost as Rental_Cost, R.DateRented, R.DateReturned From RentedMovies R join Customer C on R.CustIDFK=C.CustID join Movies M on R.MovieIDFK=M.MovieID";
                String Sql_Customers = "Select CustID,FirstName,LastName, Address, Phone From Customer";
                cmd = new SqlCommand(Sql_Movies,conn);
                SqlDataAdapter Adp_Movies = new SqlDataAdapter(cmd);

                DataTable Data_Grid = new DataTable();                 // Creating data Table to Store the Data Of Movies.
                Adp_Movies.Fill(Data_Grid);
                moviesGrid.DataSource = Data_Grid;

                cmd = new SqlCommand(Sql_Rentals, conn);
                SqlDataAdapter Adp_Rentals = new SqlDataAdapter(cmd);    
                DataTable Data_Grid1 = new DataTable();                 // Creating data Table to Store the Data Of RentalMovies..
                Adp_Rentals.Fill(Data_Grid1);
                RentalGrid.DataSource = Data_Grid1;


                cmd = new SqlCommand(Sql_Customers, conn);
                SqlDataAdapter adpCus = new SqlDataAdapter(cmd);
                DataTable Data_Grid2 = new DataTable();                // Creating data Table to Store the Data Of Customers.
                adpCus.Fill(Data_Grid2);
                CustGrid.DataSource = Data_Grid2;
                // dataReader = cmd.ExecuteReader();

                Show_Lists = true;



            }
            catch(Exception e)
            {
                if (Connection == false)                     // Checking the connection 
                    conn = null;
                Show_Lists = false;
                MessageBox.Show("Connection Error");          
            }
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void AddressCust_TextChanged(object sender, EventArgs e)
        {

        }

        private void Cust_ID_TextChanged(object sender, EventArgs e)
        {
            
        }
        String First_Name = "", Last_Name = "",Title="";
        String Issue_Date = "", Return_Date = "";
        DateTime iss_Date,rtn_Date;

        // After clicking on RentalGrid_CellContent, it will Select phone number of Customer and storing it to the Mobile 
        private void RentalGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)    
        {
            String Mobile = "";
            First_Name = (string)RentalGrid[1, e.RowIndex].Value;
            Last_Name = (string)RentalGrid[2, e.RowIndex].Value;
            Title = (string)RentalGrid[4, e.RowIndex].Value;
            String Phone_No = "Select Phone From Customer Where FirstName='"+First_Name+"' and LastName='"+Last_Name+"'";
            SqlCommand cmd = new SqlCommand(Phone_No, conn);
            SqlDataReader dataReader= cmd.ExecuteReader();
            if(dataReader.Read())
            {
                Mobile = dataReader.GetString(0);
            }
            dataReader.Close();
            Cust_ID.Text = RentalGrid[0, e.RowIndex].Value.ToString();
            FirstNameCust.Text = First_Name;
            LastName_Cust.Text = Last_Name;
            AddressCust.Text = (string)RentalGrid[3, e.RowIndex].Value;
            PhoneCust.Text = Mobile;
            Issue_Date = RentalGrid[6, e.RowIndex].Value.ToString();
            iss_Date = Convert.ToDateTime(Issue_Date);
            Return_Date = RentalGrid[7, e.RowIndex].Value.ToString();
            rtn_Date = Convert.ToDateTime(Return_Date);
            IssueMovieDate.Text = Issue_Date;
            ReturnedDate.Text = Return_Date;


        }


        // After clicking on CustomerGrid CellContent
        String Custmer_ID = "";
        private void CustGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Custmer_ID = CustGrid[0, e.RowIndex].Value.ToString();
            Cust_ID.Text = Custmer_ID;
             FirstNameCust.Text= (string)CustGrid[1, e.RowIndex].Value;
            LastName_Cust.Text = (string)CustGrid[2, e.RowIndex].Value;
            AddressCust.Text = (string)CustGrid[3, e.RowIndex].Value;
            PhoneCust.Text = (string)CustGrid[4, e.RowIndex].Value;
        }

        //After Clicking on Delete Customer
        private void deleteCust_Click(object sender, EventArgs e)
        {
            try
            {
                String Delete_Query = "Delete From Customer where CustID =" + Custmer_ID + "";
                SqlCommand cmd = new SqlCommand(Delete_Query, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                MessageBox.Show("Customer deleted sucessfully");
                dataReader.Close();
            }
            catch(Exception)
            {
                MessageBox.Show("Select Customer Which has not rented any movies to deletes");
            }
        }

        //After Clicking on Update Customer
        private void updateButton_Click(object sender, EventArgs e)
        {
            tabControl1_movisRenatl.SelectedTab = customersTab;
            String Sql_Customers = "Select * From Customer";
            SqlCommand cmd = new SqlCommand(Sql_Customers, conn);
            SqlDataAdapter Adp_Rentals = new SqlDataAdapter(cmd);
            DataTable Data_Grid1 = new DataTable();
            Adp_Rentals.Fill(Data_Grid1);
            CustGrid.DataSource = Data_Grid1;
        }

        //After Clicking on Delete Movie
        private void button_Deletemovie_Click(object sender, EventArgs e)
        {
            try
            {
                String Delete_Query = "Delete From Movies where MovieID =" + MovId + "";
                Console.WriteLine(Delete_Query);
                SqlCommand cmd = new SqlCommand(Delete_Query, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                MessageBox.Show("Movie Deleted sucessfully");
                dataReader.Close();
            }
            catch(Exception )
            {
                MessageBox.Show("Which Movies you want to delete?\n Select Movies First ");
            }
        }

        //After Clicking on Update Movie
        private void button_updatemovie_Click(object sender, EventArgs e)
        {
            tabControl1_movisRenatl.SelectedTab = MoviesTab;
            String Sql_Customers = "Select MovieID, Title,Genre,Rental_Cost,Year From Movies";
            SqlCommand cmd = new SqlCommand(Sql_Customers, conn);
            SqlDataAdapter Adp_Rentals = new SqlDataAdapter(cmd);
            DataTable Data_Grid1 = new DataTable();
            Adp_Rentals.Fill(Data_Grid1);
            moviesGrid.DataSource = Data_Grid1;
        }

        // After Clicking on add Customer 
        private void button_AddCustomer_Click(object sender, EventArgs e)
        {
           
            String FirstName = FirstNameCust.Text;
            String LastName = LastName_Cust.Text;
            String Addrss = AddressCust.Text;
            String phone_No = PhoneCust.Text;
            // This Query will Insert the Data in Customer Table.
            String Insert_Query = "Insert into Customer ([FirstName], [LastName], [Address], [Phone]) Values('" + FirstName+"','"+LastName+"','"+Addrss+"','"+phone_No+"')";
            SqlCommand cmd = new SqlCommand(Insert_Query, conn);
            SqlDataReader dataReader = cmd.ExecuteReader();
            MessageBox.Show("Customer Added sucessfully");           
            dataReader.Close();
        }

        // After Clicking on add Movie
        private void button_Addmovie_Click(object sender, EventArgs e)
        {
            String Titlemov = TitleMovie.Text;
            String genre = GenreMovie.Text;
            String Rentalmov = RentalCostMovie.Text;
            String RelDate = ReleaseDate.Text;
            //
            String Insert_Query = "Insert into Movies ( [Title], [Genre], [Rental_Cost], [Year]) Values('" + Titlemov + "','" + genre + "','" + Rentalmov + "','" + RelDate + "')";
            SqlCommand cmd = new SqlCommand(Insert_Query, conn);
            SqlDataReader dataReader = cmd.ExecuteReader();
            MessageBox.Show("Movie Added sucessfully");
            dataReader.Close();
        }

        // After Clicking on MoviesGrid cellContent 
        String MovId = "";
        private void moviesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try { 
            MovId = moviesGrid[0, e.RowIndex].Value.ToString();
            MovieID.Text = MovId;
            TitleMovie.Text = (string)moviesGrid[1, e.RowIndex].Value;
            GenreMovie.Text = moviesGrid[2, e.RowIndex].Value.ToString();
            RentalCostMovie.Text = moviesGrid[3, e.RowIndex].Value.ToString();
            ReleaseDate.Text = moviesGrid[4, e.RowIndex].Value.ToString();
            }
            catch(Exception)
            {
                MessageBox.Show("All field must be enttered\n Table have some Empty Field ");
            }

        }

        //After Clicking on Issue Movie, below Query will execute to insert data in RentedMovies.
        private void issueMovie_Click(object sender, EventArgs e)
        {
            try
            {
                String Insert_Qry = "Insert into RentedMovies ( [MovieIDFK], [CustIDFK], [DateRented], [DateReturned]) Values(" + MovId + "," + Custmer_ID + ",'" + iss_Date + "','" + rtn_Date + "')";

                SqlCommand cmd = new SqlCommand(Insert_Qry, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Close();
                String Sql_Rentals = "Select RMID, C.FirstName as FirstName, C.LastName as LastName, C.Address as Address,M.Title as Title, M.Rental_Cost as Rental_Cost, R.DateRented, R.DateReturned From RentedMovies R join Customer C on R.CustIDFK=C.CustID join Movies M on R.MovieIDFK=M.MovieID";
                MessageBox.Show("Movie issued sucessfully");
                cmd = new SqlCommand(Sql_Rentals, conn);
                SqlDataAdapter Adp_Rentals = new SqlDataAdapter(cmd);
                DataTable Data_Grid1 = new DataTable();
                Adp_Rentals.Fill(Data_Grid1);
                RentalGrid.DataSource = Data_Grid1;
            }
            catch(Exception)
            {
                MessageBox.Show("Select Customer and Movies First to Issue the Movies and also Entered Issue date and return Date");
            }
            
        }
     
        //After Clicking on Return Movie
        private void returnMovie_Click(object sender, EventArgs e)
        {
            try
            {
                String Delete_Query = "Delete From RentedMovies where CustIDFK =(Select CustID from Customer where FirstName ='" + First_Name + "') and MovieIDFK =(Select MovieID from Movies where Title = '" + Title + "')";
                SqlCommand cmd = new SqlCommand(Delete_Query, conn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                MessageBox.Show("Customer Rented Information deleted sucessfully");
                String Sql_Rentals = "Select RMID, C.FirstName as FirstName, C.LastName as LastName, C.Address as Address,M.Title as Title, M.Rental_Cost as Rental_Cost, R.DateRented, R.DateReturned From RentedMovies R join Customer C on R.CustIDFK=C.CustID join Movies M on R.MovieIDFK=M.MovieID";
                dataReader.Close();
                cmd = new SqlCommand(Sql_Rentals, conn);
                SqlDataAdapter Adp_Rentals = new SqlDataAdapter(cmd);
                DataTable Data_Grid1 = new DataTable();
                Adp_Rentals.Fill(Data_Grid1);
                RentalGrid.DataSource = Data_Grid1;
            }
            catch(Exception)
            {
                MessageBox.Show("");
            }
        }

        // For Radio Button
        private void radioButton_Allrented_CheckedChanged(object sender, EventArgs e)
        {
            tabControl1_movisRenatl.SelectedTab = RenatlTAb;
            String Sql_Rentals = "Select RMID, C.FirstName as FirstName, C.LastName as LastName, C.Address as Address,M.Title as Title, M.Rental_Cost as Rental_Cost, R.DateRented, R.DateReturned From RentedMovies R join Customer C on R.CustIDFK=C.CustID join Movies M on R.MovieIDFK=M.MovieID";

            SqlCommand cmd = new SqlCommand(Sql_Rentals, conn);
            SqlDataAdapter Adp_Rentals = new SqlDataAdapter(cmd);
            DataTable Data_Grid1 = new DataTable();
            Adp_Rentals.Fill(Data_Grid1);
            RentalGrid.DataSource = Data_Grid1;
        }
    }
}
