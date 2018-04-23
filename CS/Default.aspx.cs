using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using System.Web.UI;
using DevExpress.Web.Internal;
using System.Drawing;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {

    }
    int lastInsertedAppointmentId;

    // DXCOMMENT: This handler is called when a datasource insert operation has been completed
    protected void SchedulingDataSource_Inserted(object sender, SqlDataSourceStatusEventArgs e) {
        // DXCOMMENT: This method saves the last inserted appointment's unique identifier
        SqlConnection connection = (SqlConnection)e.Command.Connection;
        using (SqlCommand cmd = new SqlCommand("SELECT IDENT_CURRENT('Appointments')", connection)) {
            this.lastInsertedAppointmentId = Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    // DXCOMMENT: This handler is called before appointment data is posted to the datasource for insertion
    protected void Scheduler_AppointmentRowInserting(object sender, ASPxSchedulerDataInsertingEventArgs e) {
        // DXCOMMENT: This method removes the ID field from the row insert query
        e.NewValues.Remove("ID");
    }

    // DXCOMMENT: This handler is called after a new record that contains appointment information has been inserted into the datasource
    protected void Scheduler_AppointmentRowInserted(object sender, ASPxSchedulerDataInsertedEventArgs e) {
        // DXCOMMENT: This method sets the value of the key field for the appointment's data record
        e.KeyFieldValue = this.lastInsertedAppointmentId;
    }

    // DXCOMMENT: This handler is called after appointments are added to the collection
    protected void Scheduler_AppointmentsInserted(object sender, PersistentObjectsEventArgs e) {
        // DXCOMMENT: This method sets unique identifiers for new appointments
        ((ASPxSchedulerStorage)sender).SetAppointmentId((Appointment)e.Objects[0], lastInsertedAppointmentId);
    }
    protected void AppointmentDataSource2_Inserting(object sender, SqlDataSourceCommandEventArgs e) {

    }
    protected void Scheduler_HtmlTimeCellPrepared(object handler, ASPxSchedulerTimeCellPreparedEventArgs e) {
        e.Cell.CssClass += " droppable";
    }
    protected void Scheduler_CustomCallback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
        string commandName = e.Parameter;
        if(commandName == "CreateAppointment") {
            Appointment apt = Scheduler.Storage.CreateAppointment(AppointmentType.Normal);
            DateTime startTime = Convert.ToDateTime(hf["intervalStart"]);
            DateTime endTime = Convert.ToDateTime(hf["intervalEnd"]);
            object[] rowValues = ASPxGridView1.GetRowValues(Convert.ToInt32(hf["row"]), new string[] { "ID", "TypeName", "Name", "TypeID" }) as object[];
            apt.Subject = String.Format("{1} - {0}", rowValues[2], rowValues[1]);
            apt.Description = "Test description";
            apt.ResourceId = rowValues[3];
            apt.Start = startTime;
            apt.End = endTime;
            Scheduler.Storage.Appointments.Add(apt);
        }
    }
}