using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ManushaDatabase
{
    class ManushaDatabase
    {
        private readonly SqlConnection _connection;
        public class Manusha
        {
            public int _manushaID;
            public int? _intGruntLevel;
            public string _fullName;
            public string _gruntLevel;
            public override string ToString()
            {
                return string.Format("Manusha: {0}", _fullName);
            }
        }
        public class Cake
        {
            public int _cakeID;
            public string _name;
            public int _caloricContent;
            public override string ToString()
            {
                return string.Format("Cake: {0}, Caloric content: {1}", _name, _caloricContent);
            }
        }

        public ManushaDatabase(SqlConnection connection)
        {
            _connection = connection;
        }

        public void Initialize()
        {
            string sql = @"
drop table if exists CakeConsumption;
drop table if exists Manushi;
drop table if exists Cakes;

create table Manushi (
	ManushaID int IDENTITY(1,1) PRIMARY KEY,
    IntGruntLevel int,
	FullName varchar(255),
	GruntLevel varchar(255)
);
create index Manushi_FullName
on Manushi(FullName);

create table Cakes (
	CakeID int IDENTITY(1,1) PRIMARY KEY,
	Name varchar(255),
    CaloricContent int
);
create index Cakes_CaloricContent
on Cakes(CaloricContent);

create table CakeConsumption (
	CakeConsumptionID int IDENTITY(1,1) PRIMARY KEY,
    Quantity int,
	ManushaID int FOREIGN KEY REFERENCES Manushi(ManushaID),
	CakeID int FOREIGN KEY REFERENCES Cakes(CakeID),
);
create index CakeConsumption_Quantity
on CakeConsumption(Quantity);
";
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public void InsertManusha(string fullName, string gruntLevel = "very high")
        {
            string sql = @"insert into Manushi
(FullName, GruntLevel)
values (@FullName, @GruntLevel);";
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                command.Parameters.Add(new SqlParameter("@FullName", fullName));
                command.Parameters.Add(new SqlParameter("@GruntLevel", gruntLevel));
                command.ExecuteNonQuery();
            }
        }

        public void InsertCake(string name, int caloricContent)
        {
            string sql = @"insert into Cakes
(Name, CaloricContent)
values (@Name, @CaloricContent);";
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                command.Parameters.Add(new SqlParameter("@Name", name));
                command.Parameters.Add(new SqlParameter("@CaloricContent", caloricContent));
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Manusha> ReadManushi()
        {
            string sql = "SELECT ManushaID, IntGruntLevel, FullName, GruntLevel FROM Manushi;";
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        System.Data.SqlTypes.SqlInt32 sqlIntGruntLevel = reader.GetSqlInt32(1);
                        int? intGruntLevel =
                            sqlIntGruntLevel.IsNull ? null : (int?)sqlIntGruntLevel.Value;

                        yield return new Manusha
                        {
                            _manushaID = reader.GetInt32(0),
                            _intGruntLevel = intGruntLevel,
                            _fullName = reader.GetString(2),
                            _gruntLevel = reader.GetString(3)
                        };
                    }
                }
            }
        }

        public IEnumerable<Cake> ReadCakes()
        {
            string sql = "SELECT CakeID, Name, CaloricContent FROM Cakes;";
            List<Cake> cakes = new List<Cake>();
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cakes.Add(new Cake()
                        {
                            _cakeID = reader.GetInt32(0),
                            _name = reader.GetString(1),
                            _caloricContent = reader.GetInt32(2)
                        });                        
                    }
                }
            }
            return cakes;
        }
    }
}
