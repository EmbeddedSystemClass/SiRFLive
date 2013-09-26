﻿namespace AardvarkI2CClassLibrary
{
    using System;

    public enum AardvarkI2cStatus
    {
        AA_I2C_STATUS_OK,
        AA_I2C_STATUS_BUS_ERROR,
        AA_I2C_STATUS_SLA_ACK,
        AA_I2C_STATUS_SLA_NACK,
        AA_I2C_STATUS_DATA_NACK,
        AA_I2C_STATUS_ARB_LOST,
        AA_I2C_STATUS_BUS_LOCKED,
        AA_I2C_STATUS_LAST_DATA_ACK
    }
}

