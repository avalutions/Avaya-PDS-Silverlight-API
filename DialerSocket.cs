using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Dialer.Communication
{
    internal delegate void MessageHandler(string message);

    /// <summary>
    /// DialerSocket is a wrapper class for asynchronous reading/writing to the designated host.
    /// </summary>
    internal class DialerSocket
    {
        //Large default buffer size to accommodate multiple messages in one receive
        private readonly int BUFFER_SIZE = 256;

        //Actual TcpClient that assists in exchanging messages with the host
        private Socket _client;
        private DnsEndPoint _endpoint;
        //The queue that holds messages as they are received
        internal Queue<string> _messageQueue;
        //A delegate notified of new messages
        private MessageHandler _messageHandler;
        //The actual buffer where the data is held going in and out
        private Byte[] _rawBuffer;
        //Leftovers from a partially read message
        private String _theRest;
        //Indicator that the application/api is closing
        private Boolean _isClosing;
        //Indicator that the application/api is shutting down
        private Boolean _isShutDown;

        /// <summary>
        /// Default constructor to connect to the host and begin waiting for messages immediately
        /// </summary>
        /// <param name="address">The IP address of the host.</param>
        /// <param name="port">The port of the host.</param>
        /// <param name="messageHandler">The delegate to be called when a new message is received.</param>
        internal DialerSocket(string address, int port, MessageHandler messageHandler)
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _messageHandler = messageHandler;

            _messageQueue = new Queue<string>();
            _endpoint = new DnsEndPoint(address, port, AddressFamily.InterNetwork);

            AutoResetEvent reset = new AutoResetEvent(false);
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = _endpoint;
            args.SocketClientAccessPolicyProtocol = SocketClientAccessPolicyProtocol.Tcp;
            args.Completed += (s, e) => reset.Set();
            _client.ConnectAsync(args);

            reset.WaitOne();

            Receive();
        }

        /// <summary>
        /// The method of sending a message to the server
        /// </summary>
        /// <param name="message">The ASCII message to send to the server</param>
        /// <returns>Will return true if successful, false if we cannot write to the stream for some reason</returns>
        internal bool Send(string message)
        {
            if (this._client != null && this._client.Connected)
            {
                Byte[] rawBuffer = System.Text.Encoding.UTF8.GetBytes(message);
                // Issue an asynchronus write
                SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
                sendArgs.SetBuffer(rawBuffer, 0, rawBuffer.GetLength(0));
                sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SendComplete);
                sendArgs.RemoteEndPoint = _endpoint;
                this._client.SendAsync(sendArgs);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// The method that is invoked when a message was sent.
        /// </summary>
        /// <param name="result">Default asynchronous argument for call state</param>
        private void SendComplete(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success && args.LastOperation == SocketAsyncOperation.Send)
                Console.WriteLine("Error:SocketClient: Got Exception while SendComplete");
        }

        /// <summary>
        /// The method to begin receiving.  Is called in two areas:
        /// <list type="ul">
        /// <item>When the DialerSocket is initiated/constructed.</item>
        /// <item>When the EndReceive is finished</item>
        /// </list>
        /// </summary>
        private void Receive()
        {
            //Clear the messages waiting in the queue if there are any.
            while (_messageQueue.Count > 0)
            {
                //Take the message out of the queue since we are going to send it out
                string message = _messageQueue.Dequeue();
                //ASYNCHRONOUSLY invoke the message delegate with the message from the queue
                ThreadPool.QueueUserWorkItem(new WaitCallback((state) => this._messageHandler.Invoke(message)));
            }

            this._rawBuffer = new Byte[this.BUFFER_SIZE];
            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.SetBuffer(_rawBuffer, 0, _rawBuffer.Length);
            receiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveComplete);
            this._client.ReceiveAsync(receiveArgs);
        }

        /// <summary>
        /// Called once the receive method invoked finishes.
        /// </summary>
        /// <param name="result">Default asynchronous argument for call state</param>
        private void ReceiveComplete(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                // Is the Network Stream object valid
                if (args.SocketError == SocketError.Success && args.LastOperation == SocketAsyncOperation.Receive)
                {
                    // Read the current bytes from the stream buffer
                    int bytesRecieved = args.BytesTransferred;
                    // If there are bytes to process else the connection is lost
                    if (bytesRecieved > 0)
                    {
                        try
                        {
                            //Find out what we just received
                            string messagePart = UTF8Encoding.UTF8.GetString(_rawBuffer, 0, _rawBuffer.GetLength(0));
                            //Take out any trailing empty characters from the message
                            messagePart = messagePart.Replace('\0'.ToString(), "");
                            //Concatenate our current message with any leftovers from previous receipts
                            string fullMessage = _theRest + messagePart;
                            int seperator;

                            //While the index of the seperator (LINE_END defined & initiated as private member)
                            while ((seperator = fullMessage.IndexOf((char)Messages.MessageSeperator.Terminator)) > 0)
                            {
                                //Pull out the first message available (up to the seperator index
                                string message = fullMessage.Substring(0, seperator);
                                //Queue up our new message
                                _messageQueue.Enqueue(message);
                                //Take out our line end character
                                fullMessage = fullMessage.Remove(0, seperator + 1);
                            }

                            //Save whatever was NOT a full message to the private variable used to store the rest
                            _theRest = fullMessage;

                            //Empty the queue of messages if there are any
                            while (this._messageQueue.Count > 0)
                            {
                                //Take the message out of the queue since we are going to send it out
                                string message = _messageQueue.Dequeue();
                                //ASYNCHRONOUSLY invoke the message delegate with the message from the queue
                                ThreadPool.QueueUserWorkItem(new WaitCallback((state) => this._messageHandler.Invoke(message)));
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        // Wait for a new message

                        if (this._isClosing != true)
                            Receive();
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:SocketClient: Got Exception while ReceiveComplite");
            }
        }

        /// <summary> 
        /// Function used to disconnect from the server 
        /// </summary>
        public void Disconnect()
        {
            // Close down the connection
            if (_client.Connected)
                _client.Close();
        }

        /// <summary> 
        /// Finialize 
        /// </summary>
        ~DialerSocket()
        {
            if (!this._disposed)
                Dispose();
        }

        private bool _disposed;

        /// <summary>
        /// Called by the Garbage Collector (GC) to dispose of any objects needing to be released.
        /// </summary>
        public void Dispose()
        {
            try
            {
                // Flag that dispose has been called
                this._disposed = true;
                // Disconnect the client from the server
                Disconnect();
            }
            catch
            {
            }
        }
    }
}
