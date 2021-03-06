﻿namespace MartaTestEquipment
{

    internal partial class ProtocolPTU
    {
        /// <summary>
        /// This information is passed to the embedded target to inform the target whether the 
        /// request is a command request (no response besides ACK) or a data request (data message
        /// expected). Currently, this field in the header is unused by both this application and the
        /// embedded target but is provided for future use
        /// </summary>
        public enum ResponseType
        {
            COMMANDRESPONSE = 1,
            DATARESPONSE = 2,
        }

        /// <summary>
        /// This enumeration's values match exactly with those on the embedded target. These are the commands
        /// that are used to communicate information from the embedded target. Some are unused, but are included
        /// for the sake of completeness.
        /// </summary>
        public enum PacketType
        {
            SET_WATCH_ELEMENT = 2,
            SET_WATCH_ELEMENTS = 3,
            UPDATE_WATCH_ELEMENTS = 4,
            SET_CHART_SCALE = 5,
            //
            // Intentionally empty [6]
            //
            SEND_VARIABLE_VALUE = 7,

            GET_DICTIONARY_SIZE = 8,
            GET_VARIABLE_INFORMATION = 9,
            GET_EMBEDDED_INFORMATION = 10,
            GET_CHART_MODE = 11,
            SET_CHART_MODE = 12,
            GET_CHART_INDEX = 13,
            SET_CHART_INDEX = 14,
            GET_WATCH_VALUES = 15,
            GET_TIME_DATE = 16,
            SET_TIME_DATE = 17,
            START_SELF_TEST_TASK = 18,
            SELF_TEST_COMMAND = 19,
            GET_SELF_TEST_PACKET = 20,
            EXIT_SELF_TEST_TASK = 21,
            SET_FAULT_LOG = 22,
            GET_FAULT_INDICES = 23,
            GET_FAULT_HISTORY = 24,
            GET_FAULT_DATA = 25,
            GET_FAULT_FLAG = 26,
            SET_FAULT_FLAG = 27,
            //
            // Intentionally empty [28-30]
            //
            GET_DATALOG_STATUS = 31,

            GET_DATALOG_BUFFER = 32,
            //
            // Intentionally empty [33]
            //
            SET_CARID = 34,

            CLEAR_EVENTLOG = 35,
            INITIALIZE_EVENTLOG = 36,
            SET_STREAM_INFORMATION = 37,
            GET_STREAM_INFORMATION = 38,
            GET_DEFAULT_STREAM = 39,
            //
            // Intentionally empty [40-49]
            //
            START_CLOCK = 50,

            STOP_CLOCK = 51,
            CHANGE_EVENT_LOG = 52,
            GET_EVENT_LOG = 53,
            GET_STREAM_FLAG = 54,
            BTU = 55,
            //
            // Intentionally empty [56-99]
            //
            INITIALIZECOMMPORT = 100,

            CLOSECOMMPORT = 101,
            TERMINATECONNECTION = 102,
        }

        /// <summary>
        /// This enumeration matches the enumeration type on the embedded target. When variable information
        /// is passed back and forth between this application and the embedded target, a way of determining 
        /// the size of the variable is sometimes needed. 
        /// </summary>
        public enum VariableType
        {
            UINT_8_TYPE = 0,
            UINT_16_TYPE = 1,
            UINT_32_TYPE = 2,
            INT_8_TYPE = 3,
            INT_16_TYPE = 4,
            INT_32_TYPE = 5,
        }

        /// <summary>
        /// This enumeration indicates whether a variable returned from the embedded target
        /// is a signed or unsigned value.
        /// </summary>
        public enum IntegerType
        {
            SIGNED = 1,
            UNSIGNED = 2,
        }
    }
}