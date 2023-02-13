Imports System.Runtime.Remoting.Channels.Http

Public Class Client
    Inherits System.Windows.Forms.Form

    Private theManager As InBetween.Manager

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        Dim chan As HttpChannel

        'Create a HTTP channel for our use
        'IMPORTANT: Make sure you don't have anything running on this channel!
        chan = New HttpChannel("8888")

        'Registers a channel with the channel services.
        System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(chan)

        'Creates a proxy for a currently running remote object
        'This remote object is the our InBetween.Manager's instance
        'NOTE: Change 'UP' to your Chat server's name
        'http://UP:7777/ChatApplication
        theManager = CType(Activator.GetObject(Type.GetType("InBetween.Manager,InBetween"), "http://UP:7777/ChatApplication"), InBetween.Manager)

        'Add our event handler here
        'In other words, tell this fellar, "when you receive an event called evtReceiveText
        '(of type InBetween.Manager), then use the sub called HandleReceivedMsg
        'to handle it
        Try
            AddHandler Me.theManager.evtReceiveText, AddressOf Me.HandleReceivedMsg
        Catch e1 As Exception
            'Our simple exception handler
            MessageBox.Show(e1.Message)
        End Try

        'Cosmetic, I'm against it, but...
        '(This displays a caption on your client window that says "Client on <PC NAME>")
        Me.Text = "Client on " & Windows.Forms.SystemInformation.ComputerName()

        'Now, you would notice that the getHash(), will return a string that identifies 
        'the 'theManager's hash code. This Hash code will appear on ALL clients. 
        'Why? Simple, we are dealing with ONE and only ONE instance of InBetween's Manager class
        'We specified singleton on the server (Module1.vb), remember?
        'It's easy to remember, a 'single' 'ton', "SINGLE-TON"
        MessageBox.Show(Me.theManager.getHash())
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents txtReceivedMsgs As System.Windows.Forms.TextBox
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents txtMsgToSend As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.txtReceivedMsgs = New System.Windows.Forms.TextBox()
        Me.txtMsgToSend = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(280, 160)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.TabIndex = 0
        Me.btnSend.Text = "&Send"
        '
        'txtReceivedMsgs
        '
        Me.txtReceivedMsgs.Location = New System.Drawing.Point(0, 8)
        Me.txtReceivedMsgs.Multiline = True
        Me.txtReceivedMsgs.Name = "txtReceivedMsgs"
        Me.txtReceivedMsgs.ReadOnly = True
        Me.txtReceivedMsgs.Size = New System.Drawing.Size(360, 88)
        Me.txtReceivedMsgs.TabIndex = 1
        Me.txtReceivedMsgs.Text = ""
        '
        'txtMsgToSend
        '
        Me.txtMsgToSend.Location = New System.Drawing.Point(0, 104)
        Me.txtMsgToSend.Multiline = True
        Me.txtMsgToSend.Name = "txtMsgToSend"
        Me.txtMsgToSend.Size = New System.Drawing.Size(360, 48)
        Me.txtMsgToSend.TabIndex = 2
        Me.txtMsgToSend.Text = ""
        '
        'Client
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(360, 189)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.txtMsgToSend, Me.txtReceivedMsgs, Me.btnSend})
        Me.MaximizeBox = False
        Me.Name = "Client"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Client"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        'What happenning here?
        'Once we press our 'Send' button,
        'we raise an event via SendText method of Manager (InBetween)
        'Then just, erase our textbox - txtMsgToSend
        'Easy isn't it? 
        'To follow the event, peek at InBetween's Manager's SendText method
        Me.theManager.SendText(Windows.Forms.SystemInformation.ComputerName, Me.txtMsgToSend.Text)
        txtMsgToSend.Text = ""
    End Sub



    Sub HandleReceivedMsg(ByVal username As String, ByVal text As String)
        'Ok, here's what happens...
        'John Doe sends u a message, the Manager object raises an event,
        'your client intercepts it, and execution drops down here...
        'You then append the text here...
        '"... and I thought chat programs were hard..." well anyway, here's the line that does it
        Me.txtReceivedMsgs.AppendText(username & " : " & text & vbCrLf)
    End Sub

    Private Sub Client_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Try
            'Ok, let's undo what we've done
            'We've added a handler, (remember?), so now we need to remove it
            'You basically do this, ...
            RemoveHandler theManager.evtReceiveText, AddressOf Me.HandleReceivedMsg
        Catch e1 As Exception
            'Exception handling for... err, simple ppl like us...
            MessageBox.Show(e1.Message)
        End Try
    End Sub

    Private Sub Client_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
