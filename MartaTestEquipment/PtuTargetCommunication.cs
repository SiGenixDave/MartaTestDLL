using System;

namespace MartaTestEquipment
{
    /// <summary>
    /// Class that implements calls to handle the PTU to embedded target communication. It uses 
    /// </summary>
    internal class PtuTargetCommunication
    {
        /// <summary>
        /// This method is used to send a command to the embedded PTU target using the type of
        /// device specified in the argument. The difference between this method and the 3 parameter
        /// method of the same name is that this method is used when there is no payload with the command.
        /// </summary>
        /// <param name="commDevice">The comm device used to communicate with target</param>
        /// <param name="packetRequestType">The command sent to the target</param>
        /// <returns>0 (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public Int32 SendCommandToEmbedded(ICommDevice commDevice, ProtocolPTU.PacketType packetRequestType)
        {
            // Send the SOM and receive it
            Int32 commError = (Int32)commDevice.SendReceiveSOM();

            // Verify the sending and receiving of SOM is /RX OK
            if (commError != 0)
            {
                return commError;
            }

            // Create the message header for a command and command type; "null" as 1st argument indicates no payload
            ProtocolPTU.DataPacketProlog dpp = new ProtocolPTU.DataPacketProlog();
            Byte[] txMessage = dpp.GetByteArray(null, packetRequestType, ProtocolPTU.ResponseType.COMMANDRESPONSE, commDevice.IsTargetBigEndian());

            // Send the command to the target
            Int32 errorCode = commDevice.SendMessageToTarget(txMessage);

            // Verify the command was sent without errors
            if (errorCode < 0)
            {
                return errorCode;
            }

            // Since no return data is expected, verify the embedded target responds with an Acknowledge (implicit
            // acknowledge with TCP, but 232 has no such entity
            errorCode = commDevice.ReceiveTargetAcknowledge();
            if (errorCode < 0)
            {
                return errorCode;
            }

            return 0;
        }

        /// <summary>
        /// This method is used to send a command to the embedded PTU target using the type of
        /// device specified in the argument. The difference between this method and the 2 parameter
        /// method of the same name is that this method is used when there is a payload with the command.
        /// </summary>
        /// <param name="commDevice">The comm device used to communicate with target</param>
        /// <param name="requestObj">This object is a request that already has the all of the necessary payload
        /// parameters ready to be formed into a message to be sent to embedded target</param>
        /// <returns>0 (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public Int32 SendCommandToEmbedded(ICommDevice commDevice, ICommRequest requestObj)
        {
            // Send the SOM and receive it
            Int32 commError = (Int32)commDevice.SendReceiveSOM();

            // Verify the sending and receiving of SOM is /RX OK
            if (commError != 0)
            {
                return commError;
            }

            // Create the message header and payload for a command and command type
            Byte[] txMessage = requestObj.GetByteArray(commDevice.IsTargetBigEndian());

            // Send the command and payload to the target
            Int32 errorCode = commDevice.SendMessageToTarget(txMessage);

            // Verify the command was sent without errors
            if (errorCode < 0)
            {
                return errorCode;
            }

            // Since no return data is expected, verify the embedded target responds with an Acknowledge (implicit
            // acknowledge with TCP, but 232 has no such entity
            errorCode = commDevice.ReceiveTargetAcknowledge();
            if (errorCode < 0)
            {
                return errorCode;
            }

            return 0;
        }

        /// <summary>
        /// This method is used to send a data request to the embedded PTU target using the type of
        /// device specified in the argument. The difference between this method and the method of the same name
        /// is that this method is used when there is a payload with the data request.
        /// </summary>
        /// <param name="commDevice">The comm device used to communicate with target</param>
        /// <param name="requestObj">This object is a request that already has the all of the necessary payload
        /// parameters ready to be formed into a message to be sent to embedded target</param>
        /// <param name="rxMessage">Used to store the response from the embedded target</param>
        /// <returns>0 (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public Int32 SendDataRequestToEmbedded(ICommDevice commDevice, ICommRequest requestObj, Byte[] rxMessage)
        {
            // Send the SOM and receive it
            Int32 commError = (Int32)commDevice.SendReceiveSOM();

            // Verify the sending and receiving of SOM is /RX OK
            if (commError != 0)
            {
                return commError;
            }
            // Create the message header and payload for a command and command type
            Byte[] txMessage = requestObj.GetByteArray(commDevice.IsTargetBigEndian());

            // Send the command and payload to the target
            Int32 errorCode = commDevice.SendMessageToTarget(txMessage);
            if (errorCode < 0)
            {
                return -1;
            }

            // Verify the target responds with data
            errorCode = commDevice.ReceiveTargetDataPacket(rxMessage);
            if (errorCode < 0)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// This method is used to send a data request to the embedded PTU target using the type of
        /// device specified in the argument. The difference between this method and the method of the same name
        /// is that this method is used when there is NO payload with the data request.
        /// </summary>
        /// <param name="commDevice">The comm device used to communicate with target</param>
        /// <param name="packetRequestType">The command sent to the target</param>
        /// <param name="rxMessage">Used to store the response from the embedded target</param>
        /// <returns>0 (0) if all is well; otherwise another enumeration which is less than 0</returns>
        public Int32 SendDataRequestToEmbedded(ICommDevice commDevice, ProtocolPTU.PacketType packetRequestType, Byte[] rxMessage)
        {
            // Send the SOM and receive it
            Int32 commError = (Int32)commDevice.SendReceiveSOM();

            // Verify the sending and receiving of SOM is /RX OK
            if (commError != 0)
            {
                return commError;
            }

            // Create the message header for a command and command type; "null" as 1st argument indicates no payload
            ProtocolPTU.DataPacketProlog dpp = new ProtocolPTU.DataPacketProlog();
            Byte[] txMessage = dpp.GetByteArray(null, packetRequestType, ProtocolPTU.ResponseType.DATARESPONSE, commDevice.IsTargetBigEndian());

            // Send the command to the target
            Int32 errorCode = commDevice.SendMessageToTarget(txMessage);
            if (errorCode < 0)
            {
                return -1;
            }

            // Verify the target responds with data
            errorCode = commDevice.ReceiveTargetDataPacket(rxMessage);
            if (errorCode < 0)
            {
                return -1;
            }

            return 0;
        }
    }
}