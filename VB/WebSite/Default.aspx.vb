Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

	End Sub
	Private lastInsertedAppointmentId As Integer

	' DXCOMMENT: This handler is called when a datasource insert operation has been completed
	Protected Sub SchedulingDataSource_Inserted(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
		' DXCOMMENT: This method saves the last inserted appointment's unique identifier
		Dim connection As SqlConnection = CType(e.Command.Connection, SqlConnection)
		Using cmd As New SqlCommand("SELECT IDENT_CURRENT('Appointments')", connection)
			Me.lastInsertedAppointmentId = Convert.ToInt32(cmd.ExecuteScalar())
		End Using
	End Sub

	' DXCOMMENT: This handler is called before appointment data is posted to the datasource for insertion
	Protected Sub Scheduler_AppointmentRowInserting(ByVal sender As Object, ByVal e As ASPxSchedulerDataInsertingEventArgs)
		' DXCOMMENT: This method removes the ID field from the row insert query
		e.NewValues.Remove("ID")
	End Sub

	' DXCOMMENT: This handler is called after a new record that contains appointment information has been inserted into the datasource
	Protected Sub Scheduler_AppointmentRowInserted(ByVal sender As Object, ByVal e As ASPxSchedulerDataInsertedEventArgs)
		' DXCOMMENT: This method sets the value of the key field for the appointment's data record
		e.KeyFieldValue = Me.lastInsertedAppointmentId
	End Sub

	' DXCOMMENT: This handler is called after appointments are added to the collection
	Protected Sub Scheduler_AppointmentsInserted(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
		' DXCOMMENT: This method sets unique identifiers for new appointments
		CType(sender, ASPxSchedulerStorage).SetAppointmentId(CType(e.Objects(0), Appointment), lastInsertedAppointmentId)
	End Sub
	Protected Sub Scheduler_InitNewAppointment(ByVal sender As Object, ByVal e As AppointmentEventArgs)
		If hf.Contains("CustomInsertion") AndAlso CBool(hf("CustomInsertion")) = True Then
			Scheduler.JSProperties("cp_resetHf") = True
			Dim rowValues() As Object = TryCast(ASPxGridView1.GetRowValues(Convert.ToInt32(hf("row")), New String() { "ID", "TypeName", "Name", "Date", "TypeID" }), Object())
			e.Appointment.Subject = String.Format("{1} - {0}", rowValues(2), rowValues(1))
			e.Appointment.Description = "Test description"
			e.Appointment.ResourceId = rowValues(4)
			'insert your custom dates below
			Dim [date] As DateTime = Convert.ToDateTime(rowValues(3))
			e.Appointment.Start = [date]
			e.Appointment.End = [date].AddHours(1)
		End If
	End Sub
	Protected Sub AppointmentDataSource2_Inserting(ByVal sender As Object, ByVal e As SqlDataSourceCommandEventArgs)

	End Sub
End Class