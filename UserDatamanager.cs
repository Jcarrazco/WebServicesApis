using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Academy.Apis.Common;

namespace Academy.Apis.Data.DataManagers
{
    public class UserDatamanager
    {
        string connectionString = "Server=HGDLAPCARRASCOJ\\SQLEXPRESS;DataBase=UserAccount;Trusted_Connection=True";
        string createUser = "CreateUsers @Name, @LastName, @Email, @NickName, @UserStatus ";
        static UserDatamanager()
        {

        }
        public OperationResult<int> CreateUser(DataObjects.UserDO UserData)
        {
            OperationResult<int> _opResult = new OperationResult<int>() { OpStatus = 0, OpMessage = "No se logro ejecutar el SP", OpResult = -1 };
            
            try
            {
                if (null == UserData)
                {
                    throw new ArgumentNullException("UserData");
                }
                using (SqlConnection _sqlConn = new SqlConnection(connectionString))
                {
                    _sqlConn.Open();

                    using (SqlCommand _sqlCommand = new SqlCommand(createUser, _sqlConn))
                    {
                        _sqlCommand.Parameters.Add("Name", SqlDbType.VarChar);
                        _sqlCommand.Parameters.Add("LastName", SqlDbType.VarChar);
                        _sqlCommand.Parameters.Add("Email", SqlDbType.VarChar);
                        _sqlCommand.Parameters.Add("NickName", SqlDbType.VarChar);
                        _sqlCommand.Parameters.Add("UserStatus", SqlDbType.TinyInt);

                        _sqlCommand.Parameters["Name"].Value = UserData.Name;
                        _sqlCommand.Parameters["LastName"].Value = UserData.LastName;
                        _sqlCommand.Parameters["Email"].Value = UserData.Email;
                        _sqlCommand.Parameters["NickName"].Value = UserData.NickName;
                        _sqlCommand.Parameters["UserStatus"].Value = 1;

                        using (IDataReader _reader= _sqlCommand.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleRow))
                        {
                            if (_reader.Read())
                            {
                                _opResult.OpStatus = (byte)_reader[0];
                                _opResult.OpMessage = (string)_reader[1];
                                _opResult.OpResult = (int)_reader[2];
                            }
                        }
                        ;

                    }
                }

            }
            catch (Exception ex)
            {
                _opResult.OpStatus = 2;
                _opResult.OpMessage = ex.Message;
            }
            return _opResult;

        }


    }
}
