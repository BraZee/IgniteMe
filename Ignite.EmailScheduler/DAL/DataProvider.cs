using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;


namespace Ignite.EmailScheduler.DAL
{
    class DataProvider
    {
        readonly SqlConnection igniteConn;
        readonly SqlConnection emailConn;
        private readonly double _timeOut;


        public DataProvider()
        {
            string igniteConnString = ConfigurationManager.ConnectionStrings["IgniteDBConnString"].ConnectionString;
            string emailConnString = ConfigurationManager.ConnectionStrings["EmailDBConnString"].ConnectionString;

            igniteConn = new SqlConnection(igniteConnString);
            emailConn = new SqlConnection(emailConnString);

            _timeOut = double.Parse(ConfigurationManager.AppSettings["TimeOut"]);


        }



        public EmailDataSet GetEventAlertEmailsByDate(DateTime sendDate)
        {
            var retDataset = new EmailDataSet();

            
                try
                {
                    if (igniteConn.State != System.Data.ConnectionState.Open) { igniteConn.Open(); }
                    SqlCommand cmd = igniteConn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spGetEventAlertEmailByDate";

                   
                        var p = new SqlParameter("sendDate",sendDate);
                    
                    
                    cmd.Parameters.Add(p);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var sysParam = retDataset.Email.NewEmailRow();

                            sysParam.Id = int.Parse(reader["Id"].ToString());
                            sysParam.Body = reader["Body"].ToString();
                            sysParam.Subject = reader["Subject"].ToString();
                            sysParam.isUser = bool.Parse(reader["isUser"].ToString());
                            sysParam.Location  = reader["Location"].ToString();
                            sysParam.EventId = int.Parse(reader["EventId"].ToString());
                            sysParam.Event = reader["Event"].ToString();
                            sysParam.BeginDate = DateTime.Parse(reader["BeginDate"].ToString());
                            sysParam.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                            sysParam.BeginTime = TimeSpan.Parse(reader["BeginTime"].ToString());
                            sysParam.EndTime = TimeSpan.Parse(reader["EndTime"].ToString());
                            sysParam.Organisation = reader["Organisation"].ToString();

                            retDataset.Email.AddEmailRow(sysParam);
                        }
                        reader.Close();
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (igniteConn.State != System.Data.ConnectionState.Closed)
                    {
                        igniteConn.Close();
                    }
                }
            
            return retDataset;
        }


        public Dictionary<string,string> GetCustomerDetailsByEventId(int eventId)
        {
            var retDataset = new Dictionary<string, string>();


            try
            {
                if (igniteConn.State != System.Data.ConnectionState.Open) { igniteConn.Open(); }
                SqlCommand cmd = igniteConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetEventCustomersByEventId";


                var p = new SqlParameter("eventId", eventId);


                cmd.Parameters.Add(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        
                        retDataset.Add(reader["FirstName"].ToString(),reader["Email"].ToString());
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (igniteConn.State != System.Data.ConnectionState.Closed)
                {
                    igniteConn.Close();
                }
            }

            return retDataset;
        }


        public Dictionary<string, string> GetEventCoordinatorDetailsByEventId(int eventId)
        {
            var retDataset = new Dictionary<string, string>();


            try
            {
                if (igniteConn.State != System.Data.ConnectionState.Open) { igniteConn.Open(); }
                SqlCommand cmd = igniteConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetEventCoordinatorsByEventId";


                var p = new SqlParameter("eventId", eventId);


                cmd.Parameters.Add(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {

                        retDataset.Add(reader["Name"].ToString(), reader["Email"].ToString());
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (igniteConn.State != System.Data.ConnectionState.Closed)
                {
                    igniteConn.Close();
                }
            }

            return retDataset;
        }


        public Dictionary<decimal, string> GetTicketPriceByEventId(int eventId)
        {
            var retDataset = new Dictionary<decimal, string>();


            try
            {
                if (igniteConn.State != System.Data.ConnectionState.Open) { igniteConn.Open(); }
                SqlCommand cmd = igniteConn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "spGetAllEventTicketPricesByEvent";


                var p = new SqlParameter("eventId", eventId);


                cmd.Parameters.Add(p);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {

                        retDataset.Add(decimal.Parse(reader["Price"].ToString()), reader["Name"].ToString());
                    }
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (igniteConn.State != System.Data.ConnectionState.Closed)
                {
                    igniteConn.Close();
                }
            }

            return retDataset;
        }



        public void MarkMessageAsProcessed(bool isEmail,int id)
        {
            int newId = -1;
            
            try
                {
                    if (igniteConn.State != System.Data.ConnectionState.Open) { igniteConn.Open(); }
                    SqlCommand cmd = igniteConn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (isEmail)
                    {
                        cmd.CommandText = "spSetAlertEmailAsProcessed";
                    }
                    else
                    {
                        cmd.CommandText = "spSetAlertSMSAsProcessed";
                    }

                    var p2 = new SqlParameter[2];
                    for (int i = 0; i < 2; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("id", id);
                    p2[1] = new SqlParameter("updateUser", 1);
                    

                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());
                    if (igniteConn.State != System.Data.ConnectionState.Closed)
                    {
                        igniteConn.Close();
                    }
                    
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (igniteConn.State != System.Data.ConnectionState.Closed)
                    {
                        igniteConn.Close();
                    }
                }
            
            //return newId;
        }

        public int AddEmailOut(EmailOutDataSet sysParameter)
        {
            int newId = -1;
            
                try
                {
                    if (emailConn.State != System.Data.ConnectionState.Open) { emailConn.Open(); }
                    SqlCommand cmd = emailConn.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "spAddEmailOut";

                    var p2 = new SqlParameter[12];
                    for (int i = 0; i < 12; i++)
                        p2[i] = new SqlParameter();

                    p2[0] = new SqlParameter("senderName", sysParameter.EmailOut[0].SenderName);
                    p2[1] = new SqlParameter("subject", sysParameter.EmailOut[0].Subject);
                    p2[2] = new SqlParameter("recipient", sysParameter.EmailOut[0].Recipient);
                    p2[3] = new SqlParameter("cc", sysParameter.EmailOut[0].CC);
                    p2[4] = new SqlParameter("bcc", sysParameter.EmailOut[0].BCC);
                    p2[5] = new SqlParameter("message", sysParameter.EmailOut[0].Message);
                    p2[6] = new SqlParameter("gatewayId", sysParameter.EmailOut[0].GatewayId);
                    p2[7] = new SqlParameter("attachments", sysParameter.EmailOut[0].Attachments);
                    p2[8] = new SqlParameter("status", sysParameter.EmailOut[0].Status);
                    p2[9] = new SqlParameter("errorCode", sysParameter.EmailOut[0].ErrorCode);
                    p2[10] = new SqlParameter("e_datetime", sysParameter.EmailOut[0].EDateTime);
                    p2[11] = new SqlParameter("s_datetime", sysParameter.EmailOut[0].SDateTime);
                    
                    cmd.Parameters.AddRange(p2);

                    newId = int.Parse(cmd.ExecuteScalar().ToString());

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (emailConn.State != System.Data.ConnectionState.Closed)
                    {
                        emailConn.Close();
                    }
                }
            
            return newId;
        }

        
    }
}
