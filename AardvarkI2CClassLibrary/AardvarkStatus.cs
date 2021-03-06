﻿namespace AardvarkI2CClassLibrary
{
    using System;

    public enum AardvarkStatus
    {
        AA_COMMUNICATION_ERROR = -6,
        AA_CONFIG_ERROR = -10,
        AA_GPIO_NOT_AVAILABLE = -400,
        AA_I2C_BUS_ALREADY_FREE = -108,
        AA_I2C_DROPPED_EXCESS_BYTES = -107,
        AA_I2C_MONITOR_NOT_AVAILABLE = -500,
        AA_I2C_MONITOR_NOT_ENABLED = -501,
        AA_I2C_NOT_AVAILABLE = -100,
        AA_I2C_NOT_ENABLED = -101,
        AA_I2C_READ_ERROR = -102,
        AA_I2C_SLAVE_BAD_CONFIG = -104,
        AA_I2C_SLAVE_READ_ERROR = -105,
        AA_I2C_SLAVE_TIMEOUT = -106,
        AA_I2C_WRITE_ERROR = -103,
        AA_INCOMPATIBLE_DEVICE = -5,
        AA_INCOMPATIBLE_LIBRARY = -4,
        AA_INVALID_HANDLE = -9,
        AA_OK = 0,
        AA_SPI_DROPPED_EXCESS_BYTES = -205,
        AA_SPI_NOT_AVAILABLE = -200,
        AA_SPI_NOT_ENABLED = -201,
        AA_SPI_SLAVE_READ_ERROR = -203,
        AA_SPI_SLAVE_TIMEOUT = -204,
        AA_SPI_WRITE_ERROR = -202,
        AA_UNABLE_TO_CLOSE = -8,
        AA_UNABLE_TO_LOAD_DRIVER = -2,
        AA_UNABLE_TO_LOAD_FUNCTION = -3,
        AA_UNABLE_TO_LOAD_LIBRARY = -1,
        AA_UNABLE_TO_OPEN = -7
    }
}

