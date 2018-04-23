using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;

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
    protected void Scheduler_InitNewAppointment(object sender, AppointmentEventArgs e) {
        if (hf.Contains("CustomInsertion") && (bool)hf["CustomInsertion"] == true) {
            Scheduler.JSProperties["cp_resetHf"] = true;
            object[] rowValues = ASPxGridView1.GetRowValues(Convert.ToInt32(hf["row"]), new string[] { "ID", "TypeName", "Name", "Date", "TypeID" }) as object[];
            e.Appointment.Subject = String.Format("{1} - {0}", rowValues[2], rowValues[1]);
            e.Appointment.Description = "Test description";
            e.Appointment.ResourceId = rowValues[4];
            //insert your custom dates below
            DateTime date = Convert.ToDateTime(rowValues[3]);
            e.Appointment.Start = date;
            e.Appointment.End = date.AddHours(1);
        }
    }
    protected void AppointmentDataSource2_Inserting(object sender, SqlDataSourceCommandEventArgs e) {

    }
}