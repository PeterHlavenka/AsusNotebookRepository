Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.Runtime.Remoting
Imports System
Imports InBetween


public class Server

    Public Shared Sub Main()
        Dim server1 As Server
        server1 = New Server()
    End Sub

    Public Sub New()

        'Create a HTTP channel for our use
        'We'll 'talk' on this port
        'IMPORTANT: Make sure you don't have anything running on this channel!
        Dim chan As IChannel = New HttpChannel(7777)

        'Register it
        ChannelServices.RegisterChannel(chan)

        'I could have read the config from an xml file with :
        'System.Runtime.Remoting.RemotingConfiguration.Configure(your.config.file)
        '(XML format) 
        'Refer .NET Framework Class Library RemotingConfiguration.Configure Method  [Visual Basic]
        'BUT somehow, I just couldn't make it work! So I went for this great 1 line code shown below:

        'Notice these things:
        '1. We are registering a service: RegisterWellKnownServiceType
        '2. It's of type: Type.GetType("InBetween.Manager, InBetween")
        '   InBetween is the namespace, Manager is the class
        '3. We're calling that application, ChatApplication
        '4. It's of type: Singleton
        '   Why Singleton and not singlecall? If u chose singlecall, 
        '   everyone client (John and the Jones) would be creating their own Manager objects
        '   which would mean no message ever gets across to anyone
        System.Runtime.Remoting.RemotingConfiguration.RegisterWellKnownServiceType( _
            Type.GetType("InBetween.Manager, InBetween"), _
            "ChatApplication", WellKnownObjectMode.Singleton)

        'I instantiated the Manager class and called getHash 
        'Read Manager.vb for more details on getHash
        Dim Manager1 As New Manager()
        Console.WriteLine("The Manager object's ID:" & Manager1.getHash())
        System.Console.WriteLine("Hit ENTER to exit...")

        'We don't want this object to die out too fast, so we just put a 
        'ReadLine here to sustain the object's lifetime
        System.Console.ReadLine()

    End Sub
End Class