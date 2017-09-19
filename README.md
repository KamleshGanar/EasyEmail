# EasyEmail
This makes your email functionality very easy and simple with background job to make sure email has been sent and maintain the logs.

# NuGet Package
Install-Package AT.Net.Service -Version 1.0.0 

# Email Configuration
Every email system has some basic SMTP configuration to send the email. It has some more basic configuration other than the SMTP.

# In Global.asax :
```
#region [ Email Configuration ]<br/>
Email.Configuration.Display = "Testing Email";<br/>
Email.Configuration.Username = ConfigurationManager.AppSettings["SMTP_DEFAULT_USERNAME"];<br/>
Email.Configuration.From = ConfigurationManager.AppSettings["SMTP_DEFAULT_EMAIL"];<br/>
Email.Configuration.Password = ConfigurationManager.AppSettings["SMTP_DEFAULT_PASSWORD"];<br/>
Email.Configuration.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_DEFAULT_PORT"]);<br/>
Email.Configuration.UseSSL = bool.Parse(ConfigurationManager.AppSettings["SMTP_DEFAULT_USESSL"]);<br/>
Email.Configuration.SMTP = ConfigurationManager.AppSettings["SMTP_DEFAULT_HOST"];<br/>
Email.Configuration.UseDefaultCredentials = true;<br/>
Email.Interval = int.Parse(ConfigurationManager.AppSettings["INTERVAL"]);<br/>
Email.SendTrials = 10;<br/>

string _ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;<br/>
EmailDB.ConnectionString = _ConnectionString;<br/>
string ErrorMessage = string.Empty;<br/>
if (EmailDB.CheckConnection(ref ErrorMessage))<br/>
{<br/>
    EmailDB.ConfigureDatabase();<br/>
}<br/>
Email.StartProcess();<br/>
#endregion
```

# Description :
<b>Basic configuration that requires to send the email.</b><br/>
```
Email.Configuration.Display = "Testing Email";<br/>
Email.Configuration.Username = ConfigurationManager.AppSettings["SMTP_DEFAULT_USERNAME"];<br/>
Email.Configuration.From = ConfigurationManager.AppSettings["SMTP_DEFAULT_EMAIL"];<br/>
Email.Configuration.Password = ConfigurationManager.AppSettings["SMTP_DEFAULT_PASSWORD"];<br/>
Email.Configuration.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_DEFAULT_PORT"]);<br/>
Email.Configuration.UseSSL = bool.Parse(ConfigurationManager.AppSettings["SMTP_DEFAULT_USESSL"]);<br/>
Email.Configuration.SMTP = ConfigurationManager.AppSettings["SMTP_DEFAULT_HOST"];<br/>
Email.Configuration.UseDefaultCredentials = true;<br/>
```
<b><u>The specific configuration</u></b><br/>
Email.Interval : Set the specific interval to send or try to send the email. Default interval is 10 seconds. <br/>
Email.SendTrials : Set the number of times tryies to send the email. If you set 10 then it will try to send the email 10 times. Due to some 
SMTP error if email has not been send then it will tries to send 10 times and after that set the flag with remark.<br/>

EmailDB.ConnectionString : Specify the connectionstring of database to hold the emails in queue. <br/>
string ErrorMessage = string.Empty;<br/>
EmailDB.CheckConnection(ref ErrorMessage)) : Check the database connection.<br/>
EmailDB.ConfigureDatabase(): This method will configure your database. It will automatically creates the require table, stored procedures.<br/>
Email.StartProcess() : This method will start the background process for given interval.

# How to send email
```
Email e = new Email()
{
    To = _To,
    Cc = _Cc,
    Bcc = _BCc,
    Subject = email.Subject,
    Body = email.Message,
    Format = BodyFormat.HTML,
    Priority = Priority.High
};

e.Attachments = new List<Attachment>();
// Verify that the user selected a file
if (file != null && file.ContentLength > 0)
{
    string GeneratedFilename = Guid.NewGuid().ToString("N");

    // extract only the filename
    var fileName = Path.GetFileName(file.FileName);
    // store the file inside ~/App_Data/uploads folder
    var path = Path.Combine(Server.MapPath("~/_Attachments"),
        GeneratedFilename + "_" + file.FileName);
    file.SaveAs(path);

    //  Add the attachment into email.
    Attachment FileInfo = new Attachment()
    {
        Path = "~/_Attachments",
        Filename = GeneratedFilename + "_" + file.FileName,
        OriginalFilename = fileName
    };
    e.Attachments.Add(FileInfo);
}

e.Send(true);
```

# Send() and SendAsync() Method
This two method are use to send the email and having two overloading method of each methods.<br/>
```
bool Send(bool keepInQueue)<br/>
bool Send(bool keepInQueue, string to, string cc, string bcc, string subject, AT.Net.Service.Priority priority, BodyFormat format, string body)
```
<br/><br/>
Async Methods<br>
```
SendAsync(bool keepInQueue)<br/>
SendAsync(bool keepInQueue, string to, string cc, string bcc, string subject, AT.Net.Service.Priority priority, BodyFormat format, string body)
```
<br>
If we pass keepInQueue parameter is True then it will maintain the emails in queue into the database and background job will work to send the email and log will be maintain with success and failuare status with remark and error messages.
If we pass keepInQueue parameter is false then it will send the email directly without maintaining their log.

