namespace CustomerCommLib
{
    public class CustomerComm
    {
        IMailSender _mailSender;

        public CustomerComm(IMailSender mailSender)
        {
            _mailSender = mailSender;
        }

        public bool SendMailToCustomer()
        {
            //Actual logic goes here
            //define message and mail address

            _mailSender.SendMail("cust123@abc.com", "Some Message");

            return true;
        }
    }
}
