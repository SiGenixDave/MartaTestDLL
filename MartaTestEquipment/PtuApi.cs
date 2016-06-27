/////////////////////////////////////////////////////////////////////////////////////
// File:        PtuApi.cs
// Author:      D.Smail
// Revision:    -
//
// Description: Used as an API to PTU functionality to support MARTA Test equipment.
//              A DLL is created and imported into Lab Windows which then creates
//              another "C" file to interface to this API 
/////////////////////////////////////////////////////////////////////////////////////
using System;

namespace MartaTestEquipment
{
    /// <summary>
    /// Static class (required for Lab Windows) to support necessary PTU functionality
    /// </summary>
    public static class PtuApi
    {
        // Maintains the current serial port connection
        private static Serial m_SerComm;

        // Stores the results (response) from the PTU target (COMC)
        private static ushort []m_ResultBuffer = new ushort[100];

        /// <summary>
        /// Reads the results buffer (response from the target) a word at a time
        /// </summary>
        /// <param name="result">value that is read from results buffer</param>
        /// <param name="index">index into the results buffer</param>
        public static void ReadResultBuffer(ref ushort result, ushort index)
        {
            // update the reference
            result = m_ResultBuffer[index];
        }

        /// <summary>
        /// Opens the desired Com port with the desired baud rate
        /// </summary>
        /// <param name="comPort">com port (e.g. "COM1", COM12", etc.)</param>
        /// <param name="baud">baud rate (e.g. "9600", "19200", etc.)</param>
        /// <returns>0 if successful; less than 0 if unsuccessful</returns>
        public static int InitCommunication(string comPort, string baud)
        {
            m_SerComm = new Serial();
            return m_SerComm.Open(comPort + "," + baud + ",none,8,1");
        }

        /// <summary>
        /// Closes the com port
        /// </summary>
        /// <returns>0 if successful; less than 0 if unsuccessful</returns>
        public static int CloseCommunication()
        {
            // Verify a com port was opened
            if (m_SerComm == null)
            {
                return -2;
            }
            // attempt to close it
            int retVal = m_SerComm.Close();
            // indicate to other APIs that no com port is opened
            m_SerComm = null;
            return retVal;
        }

        /// <summary>
        /// Sends and receives a BTU request/response to the target. Uses existing PTU functionality.
        /// The response is saved (the Mode isn't)
        /// </summary>
        /// <param name="mode">desired mode</param>
        /// <param name="requestBuffer">request buffer (must be populated with desired payload prior to being called)</param>
        /// <returns></returns>
        public static int PTU_MVB_Interface (ushort mode, ushort []requestBuffer)
        {
            if (m_SerComm == null)
            {
                return -2;
            }
            ushort[] payload = new ushort[17];
 
            for (int i = 0; i < payload.Length; i++)
            {
                 payload[i] = requestBuffer[i];
            }

            // Create the BTU request object with the Mode and the Request
            ProtocolPTU.BtuReq request = new ProtocolPTU.BtuReq(mode, payload);

            PtuTargetCommunication ptuTargetComm = new PtuTargetCommunication();
            // Transmit the message... the response is stored in rxMessage
            byte [] rxMessage = new byte[1024];
            int commError = ptuTargetComm.SendDataRequestToEmbedded(m_SerComm, request, rxMessage);

            // If no errors exist, store the target response starting at byte 10 (bytes 0-7 are the header 
            // bytes 8 & 9 are the "Mode")
            if (commError == 0)
            {
                for (int i = 0; i < 17; i++)
                {
                    m_ResultBuffer[i] = BitConverter.ToUInt16(rxMessage, ((i * 2) + 10));
                    if (m_SerComm.IsTargetBigEndian())
                    {
                        m_ResultBuffer[i] = Utils.ReverseByteOrder(m_ResultBuffer[i]);
                    }
                }
            }

            return commError;

        }
    }
}