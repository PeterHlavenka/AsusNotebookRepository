Imports System

'We are going to declare an event delegate 
'Event delegates sound really big... but what this is doing is that:
' "I want to be able, from this class, to raise a type of an event called 'ReceiveText..." 
' "And, I want those who use Manager class to handle that event"
' "...Now, I'm going to pass  username As String and text As String."
' "It's gonna be up to you, how you're going to handle it"
Public Delegate Sub ReceiveText(ByVal username As String, ByVal text As String)

Public Class Manager
    Inherits MarshalByRefObject

    'Why inherit MarshalByRefObject?
    'Enables access to objects across application domain boundaries 
    'in applications that support remoting.
    'Src: .NET Framework Class Library  MarshalByRefObject Class  [Visual Basic]
    'Let's break it down (in simple english)... 
    'MarshalByRefObject is the means which allows objects like this class here, to 
    'communicate across boundaries, via remoting.
    'An application domain is a partition in an operating system process where one or more applications reside.

    'What's this? I thought we already declared an event handler?
    'Here's where we need to declare the event itself. 
    'Delegates, as its name suggests are 'ambassadors' who advertises this event
    'That's why in our Client.vb, we say "theManager.evtReceiveText", and not 'theManager.ReceiveText'
    Public Event evtReceiveText As ReceiveText

    Public Overrides Function InitializeLifetimeService() As Object
        'This function, if overriden, allows us to tell the Manager object how long
        'it should live. If by any reason we'd like to have it stay a 'lil longer, we renew it and so forth
        'We won't do anything here. So what happens is that 
        'the Manager object is governed by a default lifetime.
        Return Nothing
    End Function


    Public Function SendText(ByVal username As String, ByVal text As String)
        'Later in the client.vb code, you would see that chat clients (like John Doe), will 
        'raise thise event by calling SendText with the appropriate paramaters.
        'This event is then propagated to ALL clients, like Jones and so forth.
        'On Jones' PC (for example), Client.vb would handle this event by displaying something like
        '"John: yada yada" on the txtReceivedMsgs. 
        'Of course John's client window sould also show the same
        RaiseEvent evtReceiveText(username, text)
    End Function



    Public Function getHash() As String
        'this is just a handy function to reaffirm that all your clients are communicating
        'with a ONE and only Manager object
        'Which means (in simple English), 
        'John and the Jones will see the very same hash code which was assigned to the Manager object
        Return Me.GetType.GetHashCode().ToString
    End Function

End Class
