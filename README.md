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

# Methods
````
/// <summary>
/// Method to send the email.
/// </summary>
/// <param name="keepInQueue">Provide the boolean value to proceed to keep in queue to send later by background job.</param>
/// <returns></returns>
public string Send(bool keepInQueue = true)
````
This method is use to send the email. If parameter keepInQueue is true(which is default value) then the system will hold the email information into the database and background job will proceed to send email automatically in specific defined interval. If keepInQueue is false then the system will hold the email information but proceed to send instantly. This method will return the ReferenceID.

````
/// <summary>
///  Method to send the email.
/// </summary>
/// <param name="keepInQueue">It will keep the email in queue and on time interval retrieve and send it.</param>
/// <param name="to">Email send to whome. (Semicolon(;) Separated email addressess)</param>
/// <param name="cc">Carbon copy email send to whome (Semicolon(;) Separated email addressess)</param>
/// <param name="bcc">Blind carbon copy email send to whome (Semicolon(;) Separated email addressess)</param>
/// <param name="subject">Email subjects</param>
/// <param name="priority">Set the sending priorities.</param>
/// <param name="format">Body format.</param>
/// <param name="body">Message body.</param>
/// <returns></returns>
public string Send(string to, string cc, string bcc, string subject, Priority priority, BodyFormat format, string body, bool keepInQueue = true)
````
This is an overload method which accept the email paramters directly with the method.

````
/// <summary>
/// Method to send the email asynchronously.
/// </summary>
/// <param name="keepInQueue">Provide the boolean value to proceed to keep in queue to send later by background job.</param>
/// <returns></returns>
public async Task<string> SendAsync(bool keepInQueue = true)
````
This method having the same functionality like Send(bool keepInQueue) method. The basic difference is this method will call asynchronously.

````
/// <summary>
///  Method to send the email async.
/// </summary>
/// <param name="keepInQueue">It will keep the email in queue and on time interval retrieve and send it.</param>
/// <param name="to">Email send to whome. (Semicolon(;) Separated email addressess)</param>
/// <param name="cc">Carbon copy email send to whome (Semicolon(;) Separated email addressess)</param>
/// <param name="bcc">Blind carbon copy email send to whome (Semicolon(;) Separated email addressess)</param>
/// <param name="subject">Email subjects</param>
/// <param name="priority">Set the sending priorities.</param>
/// <param name="format">Body format.</param>
/// <param name="body">Message body.</param>
/// <returns></returns>
public async Task<string> SendAsync(string to, string cc, string bcc, string subject, Priority priority, BodyFormat format, string body, bool keepInQueue = true)
````
This is overload method SendAsync which accept the email parameters directly with the method.

````
/// <summary>
/// Method returns directly sent emails.
/// </summary>
/// <param name="currentPage">Provide current page number.</param>
/// <param name="pageSize">Provide page size.</param>
/// <param name="totalRecords">Provide the output parameter which hold the total number of records.</param>
/// <returns></returns>
public List<Email> GetDirectSentMessages(int? currentPage, int? pageSize, ref int totalRecords)
````
This method will return the list of emails which are directly sent. These emails were not to be processed by the background job.

````
///<summary>
/// This method fetch the emails.
///</summary>
///<param name="currentPage">Provide current page number.</param>
///<param name="pageSize">Provide the page size.</param>
/// <param name="totalRecords">Provide the output parameter which hold the total number of records.</param>
/// <param name="includeDirectSentMessages">Provide the true/false to include/exclude the directly sent messages.</param>
public List<Email> GetEmails(int? currentPage, int? pageSize, ref int totalRecords, bool includeDirectSentMessages = false)
````
This method will return all the emails by pagewise. If the parameter includeDirectSentMessages(default value is false) is true then it will include list of directly sent emails also.

````
/// <summary>
/// Method is use to include the non in queue emails in queue.
/// </summary>
/// <param name="IDs">Provide list of ID</param>
/// <param name="errorMessage">Provide error message holding parameter.</param>
public void IncludeEmailsInQueue(List<int> IDs, ref string errorMessage)
````
By any mishappen, if the directly send emails were not received by the receiver and reported to the administractor then admin can get process to include those emails into the queue.

````
/// <summary>
/// This method fetch the emails which is sent to the specific email address.
/// </summary>
/// <param name="receiverEmailAddresses">Provide the receivers email addresses with semi colon(;) separated.</param>
/// <param name="currentPage">Provide current page number.</param>
/// <param name="pageSize">Provide the page size.</param>
/// <returns></returns>
public List<Email> GetEmails(string receiverEmailAddresses, int? currentPage, int? pageSize, ref int totalRecords, bool includeDirectSentMessages = false)
````
This method is use to get all the email of specific receiver's email address.

````
/// <summary>
/// This method return the email on the basis of reference ID.
/// </summary>
/// <param name="referenceID">Provide processed email reference ID.</param>
/// <returns></returns>
public Email GetEmail(string referenceID)
````
This method is use to get the email of specific reference ID.

````
/// <summary>
/// This method return the log of selected email on the basis of reference ID.
/// </summary>
/// <param name="referenceID">Provide processed email reference ID.</param>
/// <returns></returns>
public List<Log> GetEmailLog(string referenceID)
````
This method will return the email log of specific reference ID.
