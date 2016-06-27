
using System;
using System.IO;
using System.Text;

namespace MartaTestEquipment
{
    /// <summary>
    /// Every request from the PTU to the embedded target that has a payload must implement this
    /// interface.
    /// </summary>
    internal interface ICommRequest
    {
        /// <summary>
        /// Method that formats the message going to the embedded PTU target. The format of the message
        /// is specific to the type of request made.
        /// </summary>
        /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
        /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
        Byte[] GetByteArray(Boolean targetIsBigEndian);
    }

    internal partial class ProtocolPTU
    {
        /// <summary>
        /// This is the maximum size of the outgoing message from PTU to the embedded PTU target. This
        /// size includes the packet header. This is used to allocate memory for a payload of indeterminate
        /// size or when stream writing is required to order the payload properly.
        /// </summary>
        private const UInt16 MAX_TX_STREAM_SIZE = 1024;

        /// <summary>
        /// Used to format and create a byte array used to change the event log in the embedded PTU target PTU
        /// </summary>
        public class ChangeEventLogReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.CHANGE_EVENT_LOG;

            /// <summary>
            /// Informs the embedded PTU target that this request expects some data response
            /// in return
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.DATARESPONSE;

            /// <summary>
            /// Id of the event log to change to
            /// </summary>
            private Int16 NewEventLog;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="NewEventLog">Id of the event log to change to</param>
            public ChangeEventLogReq(Int16 NewEventLog)
            {
                this.NewEventLog = NewEventLog;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private ChangeEventLogReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.NewEventLog = Utils.ReverseByteOrder(this.NewEventLog);
                }
                // Even though "ms" is allocated MAX_TX_STREAM_SIZE, the ToArray() returns the size
                // of the array that was populated with data
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.NewEventLog);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Used to format and create a byte array used to get the variable index of the desired chart channel
        /// </summary>
        public class GetChartIndexReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.GET_CHART_INDEX;

            /// <summary>
            /// Informs the embedded PTU target that this request expects some data response
            /// in return
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.DATARESPONSE;

            /// <summary>
            /// The desired chart index to retrieve; if there 8 chart channels then this
            /// value can range from 0 to 7
            /// </summary>
            private Byte ChartIndex;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="ChartIndex">The desired chart index to retrieve</param>
            public GetChartIndexReq(Byte ChartIndex)
            {
                this.ChartIndex = ChartIndex;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private GetChartIndexReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.ChartIndex = Utils.ReverseByteOrder(this.ChartIndex);
                }

                Byte[] payload = { this.ChartIndex };

                return dpp.GetByteArray(payload, PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Issues a request to the embedded target for a data log buffer
        /// </summary>
        public class GetDatalogBufferReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.GET_DATALOG_BUFFER;

            /// <summary>
            /// Informs the embedded PTU target that this request expects some data response
            /// in return
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.DATARESPONSE;

            /// <summary>
            /// the index into the array of the desired data log
            /// </summary>
            private UInt16 DatalogIndex;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="DatalogIndex">the index in the embedded target data log array</param>
            public GetDatalogBufferReq(UInt16 DatalogIndex)
            {
                this.DatalogIndex = DatalogIndex;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private GetDatalogBufferReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.DatalogIndex = Utils.ReverseByteOrder(this.DatalogIndex);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.DatalogIndex);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Issues a request  to the embedded target for at least one fault log 
        /// </summary>
        public class GetFaultDataReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.GET_FAULT_DATA;

            /// <summary>
            /// Informs the embedded PTU target that this request expects some data response
            /// in return
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.DATARESPONSE;

            /// <summary>
            /// The starting fault log index to get faults from
            /// </summary>
            private UInt32 FaultIndex;

            /// <summary>
            /// The number of fault logs to retrieve 
            /// </summary>
            private UInt16 NumberOfFaults;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="FaultIndex">The starting fault log index to get faults from</param>
            /// <param name="NumberOfFaults">The number of fault logs to retrieve</param>
            public GetFaultDataReq(UInt32 FaultIndex, UInt16 NumberOfFaults)
            {
                this.FaultIndex = FaultIndex;
                this.NumberOfFaults = NumberOfFaults;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private GetFaultDataReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.FaultIndex = Utils.ReverseByteOrder(this.FaultIndex);
                    this.NumberOfFaults = Utils.ReverseByteOrder(this.NumberOfFaults);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.FaultIndex);
                bw.Write(this.NumberOfFaults);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Gets the number of faults detected for a specific fault from the embedded target. 
        /// </summary>
        public class GetFaultHistoryReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.GET_FAULT_HISTORY;

            /// <summary>
            /// Informs the embedded PTU target that this request expects some data response
            /// in return
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.DATARESPONSE;

            /// <summary>
            /// The fault id whose history is retrieved
            /// </summary>
            private UInt16 FaultID;
            
            /// <summary>
            /// The task id of the fault whose history is retrieved
            /// </summary>
            private UInt16 TaskID;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="TaskID">The task id of the fault whose history is retrieved</param>
            /// <param name="FaultID">The fault id whose history is retrieved</param>
            public GetFaultHistoryReq(UInt16 TaskID, UInt16 FaultID)
            {
                this.TaskID = TaskID;
                this.FaultID = FaultID;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private GetFaultHistoryReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.TaskID = Utils.ReverseByteOrder(this.TaskID);
                    this.FaultID = Utils.ReverseByteOrder(this.FaultID);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.TaskID);
                bw.Write(this.FaultID);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Issues a request to get information regarding a specific stream on the embedded
        /// target. 
        /// </summary>
        public class GetStreamInfoReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.GET_STREAM_INFORMATION;

            /// <summary>
            /// Informs the embedded PTU target that this request expects some data response
            /// in return
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.DATARESPONSE;

            /// <summary>
            /// The stream number whose information is retrieved from the embedded target
            /// </summary>
            private Int16 StreamNumber;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="StreamNumber">The stream number whose information is retrieved from the 
            /// embedded target</param>
            public GetStreamInfoReq(Int16 StreamNumber)
            {
                this.StreamNumber = StreamNumber;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private GetStreamInfoReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.StreamNumber = Utils.ReverseByteOrder(this.StreamNumber);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.StreamNumber);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Queries the embedded target about the type of variable, max and min values, chart scaling, etc.
        /// Believe this is a legacy request and is no longer used.
        /// </summary>
        public class GetVariableInfoReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.GET_VARIABLE_INFORMATION;

            /// <summary>
            /// Informs the embedded PTU target that this request expects some data response
            /// in return
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.DATARESPONSE;

            /// <summary>
            /// The data dictionary index of the variable
            /// </summary>
            private UInt16 DictionaryIndex;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="DictionaryIndex">The data dictionary index of the variable</param>
            public GetVariableInfoReq(UInt16 DictionaryIndex)
            {
                this.DictionaryIndex = DictionaryIndex;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private GetVariableInfoReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.DictionaryIndex = Utils.ReverseByteOrder(this.DictionaryIndex);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.DictionaryIndex);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Updates the desired embedded target variable with a new value.
        /// </summary>
        public class SendVariableReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SEND_VARIABLE_VALUE;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// The dictionary index of the variable to be updated
            /// </summary>
            private Int16 DictionaryIndex;
            
            /// <summary>
            /// The new variable value
            /// </summary>
            private UInt32 NewValue;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="DictionaryIndex">The dictionary index of the variable to be updated</param>
            /// <param name="NewValue">The new variable value</param>
            public SendVariableReq(Int16 DictionaryIndex, UInt32 NewValue)
            {
                this.DictionaryIndex = DictionaryIndex;
                this.NewValue = NewValue;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SendVariableReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.DictionaryIndex = (Int16)Utils.ReverseByteOrder((UInt16)this.DictionaryIndex);
                    this.NewValue = Utils.ReverseByteOrder(this.NewValue);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.DictionaryIndex);
                bw.Write(this.NewValue);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Updates the embedded target with a new car ID.
        /// </summary>
        public class SetCarIDReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SET_CARID;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// The new car ID
            /// </summary>
            private String NewCarId;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="NewCarId">The new car ID</param>
            public SetCarIDReq(UInt16 NewCarId)
            {
                // TODO may have to perform a hex formatter... TBD
                this.NewCarId = NewCarId.ToString().PadRight(11, '\0');
            }

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="NewCarId">The new car ID</param>
            public SetCarIDReq(String NewCarId)
            {
                // Pad end of string with null characters and truncate to 11 chars in case 
                // passed string was longer than 11 chars
                this.NewCarId = NewCarId.PadRight(11, '\0').Substring(0, 11);
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SetCarIDReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    // Intentionally do nothing since the only item to be converted is a string
                }

                // Convert string to byte array
                Byte[] payload = Encoding.ASCII.GetBytes(this.NewCarId);

                return dpp.GetByteArray(payload, PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Adds or changes a chart recorder output to a new variable
        /// </summary>
        public class SetChartIndexReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SET_CHART_INDEX;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// The chart recorder output index
            /// </summary>
            private Byte ChartIndex;

            /// <summary>
            /// The data dictionary index of the variable which will now be part of the chart output
            /// </summary>
            private Int16 VariableIndex;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="ChartIndex">The chart recorder output index</param>
            /// <param name="VariableIndex">The data dictionary index of the variable which will now be part of the chart output</param>
            public SetChartIndexReq(Int16 ChartIndex, Int16 VariableIndex)
            {
                this.ChartIndex = (Byte)ChartIndex;
                this.VariableIndex = VariableIndex;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SetChartIndexReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.ChartIndex = Utils.ReverseByteOrder(this.ChartIndex);
                    this.VariableIndex = Utils.ReverseByteOrder(this.VariableIndex);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.VariableIndex);
                bw.Write(this.ChartIndex);

                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Sets the chart recorder mode (full, zero, data, ramp)
        /// </summary>
        public class SetChartModeReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SET_CHART_MODE;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// The chart recorder mode (full, zero, data, ramp)
            /// </summary>
            private Byte TargetChartMode;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="TargetChartMode">The chart recorder mode (full, zero, data, ramp)</param>
            public SetChartModeReq(byte TargetChartMode)
            {
                this.TargetChartMode = TargetChartMode;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SetChartModeReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.TargetChartMode = Utils.ReverseByteOrder(this.TargetChartMode);
                }

                Byte[] payload = { this.TargetChartMode };

                return dpp.GetByteArray(payload, PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Updates the chart scaling of the desired variable
        /// </summary>
        public class SetChartScaleReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SET_CHART_SCALE;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// The dictionary index of the variable to be updated
            /// </summary>
            private Int16 DictionaryIndex;

            /// <summary>
            /// The chart recorder scaling (max value)
            /// </summary>
            private Int32 MaxScale;
            
            /// <summary>
            /// The chart recorder scaling (min value)
            /// </summary>
            private Int32 MinScale;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="DictionaryIndex">The dictionary index of the variable to be updated</param>
            /// <param name="MaxScale">The chart recorder scaling (max value)</param>
            /// <param name="MinScale">The chart recorder scaling (min value)</param>
            public SetChartScaleReq(Int16 DictionaryIndex, Int32 MaxScale, Int32 MinScale)
            {
                this.DictionaryIndex = DictionaryIndex;
                this.MaxScale = MaxScale;
                this.MinScale = MinScale;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SetChartScaleReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.DictionaryIndex = Utils.ReverseByteOrder(this.DictionaryIndex);
                    this.MaxScale = Utils.ReverseByteOrder(this.MaxScale);
                    this.MinScale = Utils.ReverseByteOrder(this.MinScale);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.DictionaryIndex);
                bw.Write(this.MaxScale);
                bw.Write(this.MinScale);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Updates the fault and trigger table on th embedded target with the desired
        /// settings for the given fault. Either enables/disables fault logging and 
        /// streaming for a given fault
        /// </summary>
        public class SetFaultFlagReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SET_FAULT_FLAG;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// Non zero to enable data logging for the fault; zero to disable
            /// </summary>
            private Byte DatalogFlag;
            
            /// <summary>
            /// Non zero to enable fault logging for the fault; zero to disable
            /// </summary>
            private Byte EnableFlag;
            
            /// <summary>
            /// the fault id
            /// </summary>
            private Int16 FaultID;
            
            /// <summary>
            /// the task id of the fault parameters to update
            /// </summary>
            private Int16 TaskID;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="TaskID">the task id of the fault parameters to update</param>
            /// <param name="FaultID">the fault id</param>
            /// <param name="EnableFlag">Non zero to enable fault logging for the fault; zero to disable</param>
            /// <param name="DatalogFlag">Non zero to enable data logging for the fault; zero to disable</param>
            public SetFaultFlagReq(Int16 TaskID, Int16 FaultID, Int16 EnableFlag, Int16 DatalogFlag)
            {
                this.TaskID = TaskID;
                this.FaultID = FaultID;
                this.EnableFlag = (Byte)EnableFlag;
                this.DatalogFlag = (Byte)DatalogFlag;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SetFaultFlagReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.TaskID = Utils.ReverseByteOrder(this.TaskID);
                    this.FaultID = Utils.ReverseByteOrder(this.FaultID);
                    this.EnableFlag = Utils.ReverseByteOrder(this.EnableFlag);
                    this.DatalogFlag = Utils.ReverseByteOrder(this.DatalogFlag);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.TaskID);
                bw.Write(this.FaultID);
                bw.Write(this.EnableFlag);
                bw.Write(this.DatalogFlag);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Enables or disables fault logging on the embedded target
        /// </summary>
        public class EnableFaultLoggingReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SET_FAULT_LOG;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// Non-zero to enable fault logging; 0 to disable fault logging
            /// </summary>
            private Byte TargetState;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="TargetState">Non-zero to enable fault logging; 0 to disable fault logging</param>
            public EnableFaultLoggingReq(Byte TargetState)
            {
                this.TargetState = TargetState;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private EnableFaultLoggingReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.TargetState = Utils.ReverseByteOrder(this.TargetState);
                }

                Byte[] payload = { this.TargetState };

                return dpp.GetByteArray(payload, PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Updates the embedded target stream information which includes the data dictionary
        /// variable index and the variable type of the variable
        /// </summary>
        public class SetStreamInfoReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SET_STREAM_INFORMATION;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// Maintains all of the new stream information
            /// </summary>
            private StreamInformation streamInformation;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="NumberOfVariables"></param>
            /// <param name="SampleRate"></param>
            /// <param name="VariableIndex"></param>
            public SetStreamInfoReq(Int16 NumberOfVariables, Int16 SampleRate, Int16[] VariableIndex)
            {
                streamInformation.NumberOfVariables = (UInt16)NumberOfVariables;
                streamInformation.SampleRate = (UInt16)SampleRate;
                streamInformation.NumberOfSamples = 0;

                streamInformation.StreamVariableInfo = new StreamVariable[NumberOfVariables];

                for (Int16 i = 0; i < NumberOfVariables; i++)
                {
                    streamInformation.StreamVariableInfo[i].Variable = (UInt16)VariableIndex[i];
                    streamInformation.StreamVariableInfo[i].VariableType = 0;
                }
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SetStreamInfoReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                UInt16 numVars = streamInformation.NumberOfVariables;

                if (targetIsBigEndian)
                {
                    streamInformation.NumberOfVariables = Utils.ReverseByteOrder(streamInformation.NumberOfVariables);
                    streamInformation.NumberOfSamples = Utils.ReverseByteOrder(streamInformation.NumberOfSamples);
                    streamInformation.SampleRate = Utils.ReverseByteOrder(streamInformation.SampleRate);

                    for (UInt16 i = 0; i < numVars; i++)
                    {
                        streamInformation.StreamVariableInfo[i].Variable = Utils.ReverseByteOrder(streamInformation.StreamVariableInfo[i].Variable);
                        streamInformation.StreamVariableInfo[i].VariableType = Utils.ReverseByteOrder(streamInformation.StreamVariableInfo[i].VariableType);
                    }
                }

                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(streamInformation.NumberOfVariables);
                bw.Write(streamInformation.NumberOfSamples);
                bw.Write(streamInformation.SampleRate);
                for (UInt16 i = 0; i < numVars; i++)
                {
                    bw.Write(streamInformation.StreamVariableInfo[i].Variable);
                    bw.Write(streamInformation.StreamVariableInfo[i].VariableType);
                }
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Updates the embedded target's real time clock with the date and time
        /// </summary>
        public class SetTimeDateReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SET_TIME_DATE;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// The day of the month
            /// </summary>
            private Byte Day;

            /// <summary>
            /// True if a 4 digit year is to be used; false for a 2 digit year
            /// </summary>
            private Boolean fourDigitYear;

            /// <summary>
            /// The hour of the day
            /// </summary>
            private Byte Hour;

            /// <summary>
            /// The minute of the hour
            /// </summary>
            private Byte Minute;

            /// <summary>
            /// The month of the year
            /// </summary>
            private Byte Month;

            /// <summary>
            /// The second of the minute
            /// </summary>
            private Byte Second;

            /// <summary>
            /// The year
            /// </summary>
            private UInt16 Year;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="fourDigitYear">True if a 4 digit year is to be used; false for a 2 digit year</param>
            /// <param name="Hour">The hour of the day</param>
            /// <param name="Minute">The minute of the hour</param>
            /// <param name="Second">The second of the minute</param>
            /// <param name="Year">The year</param>
            /// <param name="Month">The month of the year</param>
            /// <param name="Day">The day of the month</param>
            public SetTimeDateReq(Boolean fourDigitYear, Byte Hour, Byte Minute, Byte Second, UInt16 Year, Byte Month, Byte Day)
            {
                this.fourDigitYear = fourDigitYear;
                this.Hour = Hour;
                this.Minute = Minute;
                this.Second = Second;
                if (fourDigitYear)
                {
                    this.Year = Year;
                }
                else
                {
                    // Just in case a 4 digit year is passed; make sure year is from 00 - 99
                    this.Year = (UInt16)(Year % 100);
                }
                this.Month = Month;
                this.Day = Day;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SetTimeDateReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                // Save the year as a byte in case a 2 digit year is desired. Don't want byte swapping on the 4 digit
                // year and then passing it as a byte
                Byte bYear = (Byte)this.Year;

                if (targetIsBigEndian)
                {
                    this.Hour = Utils.ReverseByteOrder(this.Hour);
                    this.Minute = Utils.ReverseByteOrder(this.Minute);
                    this.Second = Utils.ReverseByteOrder(this.Second);
                    this.Year = Utils.ReverseByteOrder(this.Year);
                    this.Month = Utils.ReverseByteOrder(this.Month);
                    this.Day = Utils.ReverseByteOrder(this.Day);
                }

                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.Hour);
                bw.Write(this.Minute);
                bw.Write(this.Second);
                if (this.fourDigitYear)
                {
                    // Used to fill a word boundary
                    bw.Write((Byte)0x00);
                    bw.Write(this.Year);
                }
                else
                {
                    bw.Write(bYear);
                }
                bw.Write(this.Month);
                bw.Write(this.Day);
                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }
        
       

        /// <summary>
        /// Method updates the embedded target watch element list
        /// </summary>
        public class SetWatchElementsReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SET_WATCH_ELEMENTS;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// An array of data dictionary indexes of watch variables to be reported from the embedded
            /// target back to this application
            /// </summary>
            private Int16[] WatchElement;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="WatchElement">An array of data dictionary indexes of watch variables to be 
            /// reported from the embedded target back to this application</param>
            public SetWatchElementsReq(Int16[] WatchElement)
            {
                this.WatchElement = new Int16[WatchElement.Length];
                for (UInt16 i = 0; i < WatchElement.Length; i++)
                {
                    this.WatchElement[i] = WatchElement[i];
                }
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SetWatchElementsReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    for (UInt16 i = 0; i < this.WatchElement.Length; i++)
                    {
                        this.WatchElement[i] = Utils.ReverseByteOrder(this.WatchElement[i]);
                    }
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                for (UInt16 i = 0; i < this.WatchElement.Length; i++)
                {
                    bw.Write(this.WatchElement[i]);
                }

                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Used to request an update of all watch variable values from the embedded target
        /// </summary>
        public class UpdateWatchElementsReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.UPDATE_WATCH_ELEMENTS;

            /// <summary>
            /// Informs the embedded PTU target that this request expects some data response
            /// in return
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.DATARESPONSE;

            /// <summary>
            /// Not used by embedded target, legacy
            /// </summary>
            private Int16 ForceFullUpdate;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="ForceFullUpdate">Not used by embedded target, legacy</param>
            public UpdateWatchElementsReq(Int16 ForceFullUpdate)
            {
                this.ForceFullUpdate = ForceFullUpdate;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private UpdateWatchElementsReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                Byte[] payload = { this.ForceFullUpdate != 0 ? (Byte)1 : (Byte)0 };

                return dpp.GetByteArray(payload, PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

        /// <summary>
        /// Used to request an update of all watch variable values from the embedded target
        /// </summary>
        public class BtuReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.BTU;

            /// <summary>
            /// Informs the embedded PTU target that this request expects some data response
            /// in return
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.DATARESPONSE;

            /// <summary>
            /// 
            /// </summary>
            private UInt16 Mode;

            private UInt16[] Request;

            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="ForceFullUpdate">Not used by embedded target, legacy</param>
            public BtuReq(UInt16 Mode, UInt16[] payload)
            {
                this.Mode = Mode;

                this.Request = new UInt16[payload.Length];

                for (int i = 0; i < payload.Length; i++)
                {
                    this.Request[i] = payload[i];
                }

            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private BtuReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.Mode = Utils.ReverseByteOrder(this.Mode);
                    for (UInt16 i = 0; i < this.Request.Length; i++)
                    {
                        this.Request[i] = Utils.ReverseByteOrder(this.Request[i]);
                    }
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.Mode);
                for (UInt16 i = 0; i < this.Request.Length; i++)
                {
                    bw.Write(this.Request[i]);
                }

                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }





        /// <summary>
        /// Updates the current self test command on the embedded target. This is used to enter
        /// exit, abort, and execute self tests.
        /// </summary>
        public class SelfTestCommand : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SELF_TEST_COMMAND;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// The command id 
            /// </summary>
            private Byte CommandId;

            /// <summary>
            /// The truck id
            /// </summary>
            private Byte TruckId;

            /// <summary>
            /// The data
            /// </summary>
            private UInt16 Data;
            
            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="CommandId">The command id</param>
            /// <param name="TruckId">The truck id</param>
            /// <param name="Data">The data</param>
            public SelfTestCommand(Byte CommandId, Byte TruckId, UInt16 Data)
            {
                this.CommandId = CommandId;
                this.TruckId = TruckId;
                this.Data = Data;
            }

            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SelfTestCommand()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.CommandId = Utils.ReverseByteOrder(this.CommandId);
                    this.Data = Utils.ReverseByteOrder(this.Data);
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(this.CommandId);
                bw.Write(this.TruckId);
                bw.Write(this.Data);

                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }

        }

        /// <summary>
        /// Updates the self test list on the embedded target
        /// </summary>
        public class SelfTestUpdateListReq : ICommRequest
        {
            /// <summary>
            /// Sets the packet type used to identify the message contents
            /// </summary>
            private const PacketType PACKET_TYPE = PacketType.SELF_TEST_COMMAND;

            /// <summary>
            /// Informs the embedded PTU target that this request is a command only and expects
            /// no data response in return; only an acknowledge that the message was received
            /// </summary>
            private const ResponseType RESPONSE_TYPE = ResponseType.COMMANDRESPONSE;

            /// <summary>
            /// The command id
            /// </summary>
            private Byte CommandId;

            /// <summary>
            /// The number of tests in the test list
            /// </summary>
            private Int16 NumberOfTests;

            /// <summary>
            /// The list of tests to be executed
            /// </summary>
            private Int16[] TestList;


            /// <summary>
            /// Public constructor that is the only one permitted to create this object
            /// </summary>
            /// <param name="CommandId">The command id</param>
            /// <param name="NumberOfTests">The number of tests in the test list</param>
            /// <param name="TestList">The list of tests to be executed</param>
            public SelfTestUpdateListReq(Byte CommandId, Int16 NumberOfTests, Int16[] TestList)
            {
                this.CommandId = CommandId;
                this.NumberOfTests = NumberOfTests;
                this.TestList = new Int16[TestList.Length];

                for (UInt16 i = 0; i < TestList.Length; i++)
                {
                    this.TestList[i] = TestList[i]; 
                }

            }


            /// <summary>
            /// Private 0 argument constructor that forces the instantiation of this class
            /// to use the public constructor
            /// </summary>
            private SelfTestUpdateListReq()
            {
            }

            /// <summary>
            /// Method that formats the message going to the embedded PTU target. The format of the message
            /// is specific to the type of request made.
            /// </summary>
            /// <param name="targetIsBigEndian">true if the embedded PTU target is a Big Endian machine; false otherwise</param>
            /// <returns>ordered byte array that is to be sent to the embedded PTU target</returns>
            public Byte[] GetByteArray(Boolean targetIsBigEndian)
            {
                DataPacketProlog dpp = new DataPacketProlog();

                if (targetIsBigEndian)
                {
                    this.CommandId = Utils.ReverseByteOrder(this.CommandId);
                    this.NumberOfTests = Utils.ReverseByteOrder(this.NumberOfTests);

                    for (UInt16 i = 0; i < this.TestList.Length; i++)
                    {
                        this.TestList[i] = Utils.ReverseByteOrder(this.TestList[i]);
                    }
                }
                MemoryStream ms = new MemoryStream(MAX_TX_STREAM_SIZE);
                BinaryWriter bw = new BinaryWriter(ms);

                bw.Write(this.CommandId);
                bw.Write((Byte)0);
                bw.Write(this.NumberOfTests);

                for (UInt16 i = 0; i < this.TestList.Length; i++)
                {
                    bw.Write(this.TestList[i]);
                }

                return dpp.GetByteArray(ms.ToArray(), PACKET_TYPE, RESPONSE_TYPE, targetIsBigEndian);
            }
        }

    }
}