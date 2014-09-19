using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using Custom.LoggingWrapper;
using Ignite.EmailScheduler.DAL;


namespace Ignite.EmailScheduler
{
    class Engine
    {
        private static Logger logger = null;
        private static bool is_running = true;
        private static Thread callsListener;
        private static int retries;
        private static string gatewayId = ConfigurationManager.AppSettings["UserName"];


        public void Start()
        {
            try
            {
                retries = int.Parse(ConfigurationManager.AppSettings["Retries"]);
                logger = LogManager.CreateLogger("Custom.LoggingWrapper.LogManager");

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
            try
            {
                logger.Log(Level.DEBUG, "Starting Ignite Scheduled Email Creator...");

                is_running = true;
                // Note that there are two threads doing the processing. one for registration and one for sos calls
                callsListener = new Thread(Listen);
                callsListener.Start();

                //   expiryDateChecker.Start();
                logger.Log(Level.DEBUG, "Ignite Scheduled Email Creator : started");
            }
            catch (Exception ex)
            {
                logger.Log(Level.ERROR, ex.ToString());
                Stop();
            }
        }

        public void Stop()
        {
            if (logger != null)
                logger.Log(Level.DEBUG, "Ignite Scheduled Email Creator : stopping...");
            is_running = false;
            if (callsListener != null)
            {
                callsListener.Abort();
                callsListener = null;
            }
        }

        public static void Listen()
        {
            while (is_running)
            {
                is_running = false;
                CreateEmails();
                is_running = true;
            }
        }

        public static void CreateEmails()
        {
            try
            {
                var dataProvider = new DataProvider();
                var alertEmails = dataProvider.GetEventAlertEmailsByDate(DateTime.Now);
               
                if (alertEmails.Email.Count > 0)
                {

                    try
                    {
                        Console.WriteLine(alertEmails.Email.Count + " emails to send");
                        logger.Log(Level.INFO, alertEmails.Email.Count + " emails to send");

                        foreach (var email in alertEmails.Email)
                        {
                            dataProvider.MarkMessageAsProcessed(true,email.Id);
                            string body = email.Body;
                            string subject = email.Subject;
                            body = body.Replace("[Event Name]", email.Event);
                            body = body.Replace("[Location]", email.Location);
                            body = body.Replace("[Location]", email.Location);
                            body = body.Replace("[Begin Time]", DateTime.Parse(email.BeginTime.ToString()).ToString("h:mm tt"));
                            body = body.Replace("[Begin Date]", email.BeginDate.ToLongDateString());
                            body = body.Replace("[Site URL]", "WWW.IGNITE.COM");
                            body = body.Replace("[End Date]", email.EndDate.ToLongDateString());
                            body = body.Replace("[End Time]", DateTime.Parse(email.EndTime.ToString()).ToString("h:mm tt"));
                            subject = subject.Replace("[Event Name]", email.Event);
                            subject = subject.Replace("[Location]", email.Location);
                            subject = subject.Replace("[Location]", email.Location);
                            subject = subject.Replace("[Begin Time]", DateTime.Parse(email.BeginTime.ToString()).ToString("h:mm tt"));
                            subject = subject.Replace("[Begin Date]", email.BeginDate.ToLongDateString());
                            subject = subject.Replace("[Site URL]", "WWW.IGNITE.COM");
                            subject = subject.Replace("[End Date]", email.EndDate.ToLongDateString());
                            subject = subject.Replace("[End Time]", DateTime.Parse(email.EndTime.ToString()).ToString("h:mm tt"));

                            if (body.Contains("[Price]")) 
                            { 
                                var dicPrices = dataProvider.GetTicketPriceByEventId(email.EventId);
                                string strPrice = "";
                                foreach (var price in dicPrices)
                                {
                                    strPrice += "\n\n" +price.Value + " - " + price.Key;
                                }

                                body = body.Replace("[Price]", strPrice);
                            }
                            if (subject.Contains("[Price]"))
                            {
                                var dicPrices = dataProvider.GetTicketPriceByEventId(email.EventId);
                                string strPrice = "";
                                foreach (var price in dicPrices)
                                {
                                    strPrice += "  " + price.Value + " - " + price.Key;
                                }

                                subject = subject.Replace("[Price]", strPrice);
                            }
                            
                            if (email.isUser)
                            {
                                Dictionary<string, string> users =
                                    dataProvider.GetCustomerDetailsByEventId(email.EventId);

                                foreach (var user in users)
                                {
                                    logger.Log(Level.DEBUG, string.Format("Processing Email to '{0}' ", user.Value));
                                    Console.WriteLine(String.Format("Processing Email to '{0}' ", user.Value));

                                    body = body.Replace("[Recipient Name]",user.Key);
                                    EmailOutDataSet ds = new EmailOutDataSet();
                                    ds.EmailOut.AddEmailOutRow(1, email.Organisation,subject,
                                        user.Value, string.Empty,
                                        string.Empty, body, gatewayId, string.Empty,0, string.Empty, DateTime.Now, DateTime.Now);
                                    int result = dataProvider.AddEmailOut(ds);

                                    if (result != -1)
                                    {
                                        logger.Log(Level.DEBUG, string.Format("Email to '{0}' sent.", user.Value));
                                        Console.WriteLine(String.Format("Email to '{0}' sent.", user.Value));
                                    }
                                    else
                                    {
                                        logger.Log(Level.DEBUG, string.Format("Email to '{0}' not sent.", user.Value));
                                        Console.WriteLine(String.Format("Email to '{0}' not sent.", user.Value));
                                    }
                                }
                            }
                            else
                            {
                                Dictionary<string, string> users =
                                    dataProvider.GetEventCoordinatorDetailsByEventId(email.EventId);

                                foreach (var user in users)
                                {
                                    logger.Log(Level.DEBUG, string.Format("Processing Email to '{0}' ", user.Value));
                                    Console.WriteLine(String.Format("Processing Email to '{0}' ", user.Value));

                                    body = body.Replace("[Recipient Name]", user.Key);
                                    EmailOutDataSet ds = new EmailOutDataSet();
                                    ds.EmailOut.AddEmailOutRow(1, email.Organisation, email.Subject,
                                        user.Value, string.Empty,
                                        string.Empty, body, gatewayId, string.Empty, 0, string.Empty, DateTime.Now, DateTime.Now);
                                    int result = dataProvider.AddEmailOut(ds);

                                    if (result != -1)
                                    {
                                        logger.Log(Level.DEBUG, string.Format("Email to '{0}' sent.", user.Value));
                                        Console.WriteLine(String.Format("Email to '{0}' sent.", user.Value));
                                    }
                                    else
                                    {
                                        logger.Log(Level.DEBUG, string.Format("Email to '{0}' not sent.", user.Value));
                                        Console.WriteLine(String.Format("Email to '{0}' not sent.", user.Value));
                                    }
                                }
                            }
                         
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        logger.Log(Level.ERROR, ex.Message);
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        is_running = true;
                    }
                    logger.Log(Level.INFO, "Processing Emails completed \n");
                    Console.WriteLine("Processing Emails completed \n");
                }
            }
            catch (Exception ex)
            {

                logger.Log(Level.ERROR, ex.Message);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                is_running = true;
            }
        }


    }
}
