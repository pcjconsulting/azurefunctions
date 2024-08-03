using System;
using System.Collections;
using IBM.WMQ;

namespace azurefunctions.Functions
{
    public class MQClient
    {
        // The type of connection to use.
        const String connectionType = MQC.TRANSPORT_MQSERIES_MANAGED;

        // name of the queue manager to use (applies to all connections)
        const String qManager = "QM1";

        // The host connection (applies to client connections only).
        const String hostName = "ubuntu22vm.westus.cloudapp.azure.com(1414)";

        // The name of the channel to use (applies to client connections only).
        public const String channel = "DEV.APP.SVRCONN";

        // The user configured to access the queue.
        const String username = "app";

        // The user password.
        const String password = "temppwd";

        /// <summary>
        /// Send the specified message on the specified queue.
        /// </summary>
        /// <param name="queue">The MQQueue.</param>
        /// <param name="message">The message string.</param>
        /// <returns>An ID in a byte array.</returns>
        public static byte[] SendMessage(string queueName, string message)
        {
            // Connect to the queue manager and open the queue.
            MQQueueManager qMgr = MQClient.Init();
            if (qMgr == null)
            {
                return null;
            }
            MQQueue queue = MQClient.OpenQueue(qMgr, queueName);
            if (queue == null)
            {
                return null;
            }

            // Write an IBM MQ message, in UTF format.
            var sendMessage = new MQMessage();
            sendMessage.WriteUTF(message);

            // Specify default message options and put the message on the queue.
            var pmo = new MQPutMessageOptions();
            queue.Put(sendMessage, pmo);

            // Close the queue and disconnect from the queue manager.
            queue?.Close();
            qMgr?.Disconnect();

            return sendMessage.MessageId;
        }


        /// <summary>
        /// Get a message on the specified queue with the specified ID.
        /// </summary>
        /// <param name="queue">The queue name.</param>
        /// <param name="id">The message ID.</param>
        /// <returns>An ID in a byte array.</returns>
        public static string GetMessage(string queueName, byte[] id)
        {
            // Connect to the queue manager and open the queue.
            MQQueueManager qMgr = MQClient.Init();
            if (qMgr == null)
            {
                return null;
            }
            MQQueue queue = MQClient.OpenQueue(qMgr, queueName);
            if (queue == null)
            {
                return null;
            }

            // Create an IBM MQ message to receive the message.
            var retrievedMessage = new MQMessage
            {
                MessageId = id
            };

            // Set default get message options.
            var gmo = new MQGetMessageOptions();

            // Get the message off the queue
            queue.Get(retrievedMessage, gmo);

            // Get the UTF message text
            String msgText = retrievedMessage.ReadUTF();

            // Close the queue and disconnect from the queue manager.
            queue?.Close();
            qMgr?.Disconnect();

            return msgText;
        }

        /// <summary>
        /// Initialise the connection properties and connect to a QM.
        /// </summary>
        /// <returns>A Queue Manager.</returns>
        private static MQQueueManager Init()
        {
            var connectionProperties = new Hashtable();
            connectionProperties.Add(MQC.TRANSPORT_PROPERTY, connectionType);
            connectionProperties.Add(MQC.HOST_NAME_PROPERTY, hostName);
            connectionProperties.Add(MQC.CHANNEL_PROPERTY, channel);
            connectionProperties.Add(MQC.USER_ID_PROPERTY, username);
            connectionProperties.Add(MQC.PASSWORD_PROPERTY, password);

            // Create a connection to the queue manager using the connection properties
            var qMgr = new MQQueueManager(qManager, connectionProperties);
            return qMgr;
        }


        /// <summary>
        /// Open a queue with the specified name on the specified queue manager.
        /// </summary>
        /// <param name="qMgr">The Queue Manager.</param>
        /// <param name="queueName">The queue name.</param>
        /// <returns></returns>
        private static MQQueue OpenQueue(MQQueueManager qMgr, string queueName)
        {
            // Specify options and open the queue.
            int openOptions = MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_OUTPUT;
            MQQueue queue = qMgr.AccessQueue(queueName, openOptions);
            return queue;
        }
    }
}
