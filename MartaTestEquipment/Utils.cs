﻿using System;

namespace MartaTestEquipment
{
    /// <summary>
    /// This class contains utility functions used by PTU to VCU interface.
    /// </summary>
    internal static class Utils
    {
        #region --- Methods ---

        /// <summary>
        /// Reverses the byte order of the argument to account for big endian to little endian compatibility
        /// </summary>
        /// <param name="value">parameter whose byte order is to be reversed</param>
        /// <returns>byte order of passed argument reversed</returns>
        public static Byte ReverseByteOrder(Byte value)
        {
            return value;
        }

        /// <summary>
        /// Reverses the byte order of the argument to account for big endian to little endian compatibility
        /// </summary>
        /// <param name="value">parameter whose byte order is to be reversed</param>
        /// <returns>byte order of passed argument reversed</returns>
        public static SByte ReverseByteOrder(SByte value)
        {
            return value;
        }

        /// <summary>
        /// Reverses the byte order of the argument to account for big endian to little endian compatibility
        /// </summary>
        /// <param name="value">parameter whose byte order is to be reversed</param>
        /// <returns>byte order of passed argument reversed</returns>
        public static UInt32 ReverseByteOrder(UInt32 value)
        {
            UInt32 reversedValue;

            reversedValue = (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                            (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;

            return reversedValue;
        }

        /// <summary>
        /// Reverses the byte order of the argument to account for big endian to little endian compatibility
        /// </summary>
        /// <param name="value">parameter whose byte order is to be reversed</param>
        /// <returns>byte order of passed argument reversed</returns>
        public static Int32 ReverseByteOrder(Int32 value)
        {
            Int32 reversedValue = 0;
            UInt32 tempValue = (UInt32)value;

            tempValue = (tempValue & 0x000000FFU) << 24 | (tempValue & 0x0000FF00U) << 8 |
                        (tempValue & 0x00FF0000U) >> 8 | (tempValue & 0xFF000000U) >> 24;

            reversedValue = (Int32)tempValue;
            return reversedValue;
        }

        /// <summary>
        /// Reverses the byte order of the argument to account for big endian to little endian compatibility
        /// </summary>
        /// <param name="value">parameter whose byte order is to be reversed</param>
        /// <returns>byte order of passed argument reversed</returns>
        public static UInt16 ReverseByteOrder(UInt16 value)
        {
            UInt16 reversedValue;

            reversedValue = (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);

            return reversedValue;
        }

        /// <summary>
        /// Reverses the byte order of the argument to account for big endian to little endian compatibility
        /// </summary>
        /// <param name="value">parameter whose byte order is to be reversed</param>
        /// <returns>byte order of passed argument reversed</returns>
        public static Int16 ReverseByteOrder(Int16 value)
        {
            Int16 reversedValue;

            reversedValue = (Int16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);

            return reversedValue;
        }

        #endregion --- Methods ---
    }
}