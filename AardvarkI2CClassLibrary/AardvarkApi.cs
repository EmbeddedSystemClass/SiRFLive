﻿namespace AardvarkI2CClassLibrary
{
    using System;
    using System.Runtime.InteropServices;

    [CLSCompliant(true)]
    public class AardvarkApi
    {
        public const int AA_API_VERSION = 0x500;
        public const int AA_ASYNC_I2C_MONITOR = 8;
        public const int AA_ASYNC_I2C_READ = 1;
        public const int AA_ASYNC_I2C_WRITE = 2;
        public const int AA_ASYNC_NO_DATA = 0;
        public const int AA_ASYNC_SPI = 4;
        public const int AA_CONFIG_I2C_MASK = 2;
        public const int AA_CONFIG_SPI_MASK = 1;
        public const int AA_FEATURE_GPIO = 8;
        public const int AA_FEATURE_I2C = 2;
        public const int AA_FEATURE_I2C_MONITOR = 0x10;
        public const int AA_FEATURE_SPI = 1;
        public const byte AA_GPIO_DIR_INPUT = 0;
        public const byte AA_GPIO_DIR_OUTPUT = 1;
        public const byte AA_GPIO_PULLUP_OFF = 0;
        public const byte AA_GPIO_PULLUP_ON = 1;
        public const short AA_I2C_MONITOR_CMD_START = -256;
        public const short AA_I2C_MONITOR_CMD_STOP = -255;
        public const short AA_I2C_MONITOR_DATA = 0xff;
        public const short AA_I2C_MONITOR_NACK = 0x100;
        public const byte AA_I2C_PULLUP_BOTH = 3;
        public const byte AA_I2C_PULLUP_NONE = 0;
        public const byte AA_I2C_PULLUP_QUERY = 0x80;
        private static bool AA_LIBRARY_LOADED = ((AA_SW_VERSION >= 0x500) && (0x500 >= AA_REQ_API_VERSION));
        public const short AA_PORT_NOT_FREE = -32768;
        private static short AA_REQ_API_VERSION = ((short) ((c_version() >> 0x10) & 0xffff));
        public const int AA_REQ_SW_VERSION = 0x500;
        private static short AA_SW_VERSION = ((short) (c_version() & 0xffff));
        public const byte AA_TARGET_POWER_BOTH = 3;
        public const byte AA_TARGET_POWER_NONE = 0;
        public const byte AA_TARGET_POWER_QUERY = 0x80;

        public static int aa_async_poll(int aardvark, int timeout)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_async_poll(aardvark, timeout);
        }

        public static int aa_close(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_close(aardvark);
        }

        public static int aa_configure(int aardvark, AardvarkConfig config)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_configure(aardvark, config);
        }

        public static int aa_features(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_features(aardvark);
        }

        public static int aa_find_devices(int num_devices, short[] devices)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            int num = (int) tp_min((long) num_devices, (long) devices.Length);
            return net_aa_find_devices(num, devices);
        }

        public static int aa_find_devices_ext(int num_devices, short[] devices, int num_ids, int[] unique_ids)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            int num = (int) tp_min((long) num_devices, (long) devices.Length);
            int num2 = (int) tp_min((long) num_ids, (long) unique_ids.Length);
            return net_aa_find_devices_ext(num, devices, num2, unique_ids);
        }

        public static int aa_gpio_change(int aardvark, short timeout)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_gpio_change(aardvark, timeout);
        }

        public static int aa_gpio_direction(int aardvark, byte direction_mask)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_gpio_direction(aardvark, direction_mask);
        }

        public static int aa_gpio_get(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_gpio_get(aardvark);
        }

        public static int aa_gpio_pullup(int aardvark, byte pullup_mask)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_gpio_pullup(aardvark, pullup_mask);
        }

        public static int aa_gpio_set(int aardvark, byte value)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_gpio_set(aardvark, value);
        }

        public static int aa_i2c_bitrate(int aardvark, int bitrate_khz)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_bitrate(aardvark, bitrate_khz);
        }

        public static int aa_i2c_bus_timeout(int aardvark, short timeout_ms)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_bus_timeout(aardvark, timeout_ms);
        }

        public static int aa_i2c_free_bus(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_free_bus(aardvark);
        }

        public static int aa_i2c_monitor_disable(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_monitor_disable(aardvark);
        }

        public static int aa_i2c_monitor_enable(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_monitor_enable(aardvark);
        }

        public static int aa_i2c_monitor_read(int aardvark, short num_bytes, short[] data)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            short num = (short) tp_min((long) num_bytes, (long) data.Length);
            return net_aa_i2c_monitor_read(aardvark, num, data);
        }

        public static int aa_i2c_pullup(int aardvark, byte pullup_mask)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_pullup(aardvark, pullup_mask);
        }

        public static int aa_i2c_read(int aardvark, short slave_addr, AardvarkI2cFlags flags, short num_bytes, byte[] data_in)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            short num = (short) tp_min((long) num_bytes, (long) data_in.Length);
            return net_aa_i2c_read(aardvark, slave_addr, flags, num, data_in);
        }

        public static int aa_i2c_read_ext(int aardvark, short slave_addr, AardvarkI2cFlags flags, short num_bytes, byte[] data_in, ref short num_read)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            short num = (short) tp_min((long) num_bytes, (long) data_in.Length);
            return net_aa_i2c_read_ext(aardvark, slave_addr, flags, num, data_in, ref num_read);
        }

        public static int aa_i2c_slave_disable(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_slave_disable(aardvark);
        }

        public static int aa_i2c_slave_enable(int aardvark, byte addr, short maxTxBytes, short maxRxBytes)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_slave_enable(aardvark, addr, maxTxBytes, maxRxBytes);
        }

        public static int aa_i2c_slave_read(int aardvark, ref byte addr, short num_bytes, byte[] data_in)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            short num = (short) tp_min((long) num_bytes, (long) data_in.Length);
            return net_aa_i2c_slave_read(aardvark, ref addr, num, data_in);
        }

        public static int aa_i2c_slave_read_ext(int aardvark, ref byte addr, short num_bytes, byte[] data_in, ref short num_read)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            short num = (short) tp_min((long) num_bytes, (long) data_in.Length);
            return net_aa_i2c_slave_read_ext(aardvark, ref addr, num, data_in, ref num_read);
        }

        public static int aa_i2c_slave_set_response(int aardvark, byte num_bytes, byte[] data_out)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            byte num = (byte) tp_min((long) num_bytes, (long) data_out.Length);
            return net_aa_i2c_slave_set_response(aardvark, num, data_out);
        }

        public static int aa_i2c_slave_write_stats(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_slave_write_stats(aardvark);
        }

        public static int aa_i2c_slave_write_stats_ext(int aardvark, ref short num_written)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_i2c_slave_write_stats_ext(aardvark, ref num_written);
        }

        public static int aa_i2c_write(int aardvark, short slave_addr, AardvarkI2cFlags flags, short num_bytes, byte[] data_out)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            short num = (short) tp_min((long) num_bytes, (long) data_out.Length);
            return net_aa_i2c_write(aardvark, slave_addr, flags, num, data_out);
        }

        public static int aa_i2c_write_ext(int aardvark, short slave_addr, AardvarkI2cFlags flags, short num_bytes, byte[] data_out, ref short num_written)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            short num = (short) tp_min((long) num_bytes, (long) data_out.Length);
            return net_aa_i2c_write_ext(aardvark, slave_addr, flags, num, data_out, ref num_written);
        }

        public static int aa_log(int aardvark, int level, int handle)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_log(aardvark, level, handle);
        }

        public static int aa_open(int port_number)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_open(port_number);
        }

        public static int aa_open_ext(int port_number, ref AardvarkExt aa_ext)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_open_ext(port_number, ref aa_ext);
        }

        public static int aa_port(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_port(aardvark);
        }

        public static int aa_sleep_ms(int milliseconds)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_sleep_ms(milliseconds);
        }

        public static int aa_spi_bitrate(int aardvark, int bitrate_khz)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_spi_bitrate(aardvark, bitrate_khz);
        }

        public static int aa_spi_configure(int aardvark, AardvarkSpiPolarity polarity, AardvarkSpiPhase phase, AardvarkSpiBitorder bitorder)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_spi_configure(aardvark, polarity, phase, bitorder);
        }

        public static int aa_spi_master_ss_polarity(int aardvark, AardvarkSpiSSPolarity polarity)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_spi_master_ss_polarity(aardvark, polarity);
        }

        public static int aa_spi_slave_disable(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_spi_slave_disable(aardvark);
        }

        public static int aa_spi_slave_enable(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_spi_slave_enable(aardvark);
        }

        public static int aa_spi_slave_read(int aardvark, short num_bytes, byte[] data_in)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            short num = (short) tp_min((long) num_bytes, (long) data_in.Length);
            return net_aa_spi_slave_read(aardvark, num, data_in);
        }

        public static int aa_spi_slave_set_response(int aardvark, byte num_bytes, byte[] data_out)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            byte num = (byte) tp_min((long) num_bytes, (long) data_out.Length);
            return net_aa_spi_slave_set_response(aardvark, num, data_out);
        }

        public static int aa_spi_write(int aardvark, short out_num_bytes, byte[] data_out, short in_num_bytes, byte[] data_in)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            short num = (short) tp_min((long) out_num_bytes, (long) data_out.Length);
            short num2 = (short) tp_min((long) in_num_bytes, (long) data_in.Length);
            return net_aa_spi_write(aardvark, num, data_out, num2, data_in);
        }

        public static string aa_status_string(int status)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return null;
            }
            return Marshal.PtrToStringAnsi(net_aa_status_string(status));
        }

        public static int aa_target_power(int aardvark, byte power_mask)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_target_power(aardvark, power_mask);
        }

        public static int aa_unique_id(int aardvark)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_unique_id(aardvark);
        }

        public static int aa_version(int aardvark, ref AardvarkVersion version)
        {
            if (!AA_LIBRARY_LOADED)
            {
                return -4;
            }
            return net_aa_version(aardvark, ref version);
        }

        [DllImport("aardvark")]
        private static extern int c_version();
        [DllImport("aardvark")]
        private static extern int net_aa_async_poll(int aardvark, int timeout);
        [DllImport("aardvark")]
        private static extern int net_aa_close(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_configure(int aardvark, AardvarkConfig config);
        [DllImport("aardvark")]
        private static extern int net_aa_features(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_find_devices(int num_devices, [Out] short[] devices);
        [DllImport("aardvark")]
        private static extern int net_aa_find_devices_ext(int num_devices, [Out] short[] devices, int num_ids, [Out] int[] unique_ids);
        [DllImport("aardvark")]
        private static extern int net_aa_gpio_change(int aardvark, short timeout);
        [DllImport("aardvark")]
        private static extern int net_aa_gpio_direction(int aardvark, byte direction_mask);
        [DllImport("aardvark")]
        private static extern int net_aa_gpio_get(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_gpio_pullup(int aardvark, byte pullup_mask);
        [DllImport("aardvark")]
        private static extern int net_aa_gpio_set(int aardvark, byte value);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_bitrate(int aardvark, int bitrate_khz);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_bus_timeout(int aardvark, short timeout_ms);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_free_bus(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_monitor_disable(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_monitor_enable(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_monitor_read(int aardvark, short num_bytes, [Out] short[] data);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_pullup(int aardvark, byte pullup_mask);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_read(int aardvark, short slave_addr, AardvarkI2cFlags flags, short num_bytes, [Out] byte[] data_in);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_read_ext(int aardvark, short slave_addr, AardvarkI2cFlags flags, short num_bytes, [Out] byte[] data_in, ref short num_read);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_slave_disable(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_slave_enable(int aardvark, byte addr, short maxTxBytes, short maxRxBytes);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_slave_read(int aardvark, ref byte addr, short num_bytes, [Out] byte[] data_in);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_slave_read_ext(int aardvark, ref byte addr, short num_bytes, [Out] byte[] data_in, ref short num_read);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_slave_set_response(int aardvark, byte num_bytes, [In] byte[] data_out);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_slave_write_stats(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_slave_write_stats_ext(int aardvark, ref short num_written);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_write(int aardvark, short slave_addr, AardvarkI2cFlags flags, short num_bytes, [In] byte[] data_out);
        [DllImport("aardvark")]
        private static extern int net_aa_i2c_write_ext(int aardvark, short slave_addr, AardvarkI2cFlags flags, short num_bytes, [In] byte[] data_out, ref short num_written);
        [DllImport("aardvark")]
        private static extern int net_aa_log(int aardvark, int level, int handle);
        [DllImport("aardvark")]
        private static extern int net_aa_open(int port_number);
        [DllImport("aardvark")]
        private static extern int net_aa_open_ext(int port_number, ref AardvarkExt aa_ext);
        [DllImport("aardvark")]
        private static extern int net_aa_port(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_sleep_ms(int milliseconds);
        [DllImport("aardvark")]
        private static extern int net_aa_spi_bitrate(int aardvark, int bitrate_khz);
        [DllImport("aardvark")]
        private static extern int net_aa_spi_configure(int aardvark, AardvarkSpiPolarity polarity, AardvarkSpiPhase phase, AardvarkSpiBitorder bitorder);
        [DllImport("aardvark")]
        private static extern int net_aa_spi_master_ss_polarity(int aardvark, AardvarkSpiSSPolarity polarity);
        [DllImport("aardvark")]
        private static extern int net_aa_spi_slave_disable(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_spi_slave_enable(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_spi_slave_read(int aardvark, short num_bytes, [Out] byte[] data_in);
        [DllImport("aardvark")]
        private static extern int net_aa_spi_slave_set_response(int aardvark, byte num_bytes, [In] byte[] data_out);
        [DllImport("aardvark")]
        private static extern int net_aa_spi_write(int aardvark, short out_num_bytes, [In] byte[] data_out, short in_num_bytes, [Out] byte[] data_in);
        [DllImport("aardvark")]
        private static extern IntPtr net_aa_status_string(int status);
        [DllImport("aardvark")]
        private static extern int net_aa_target_power(int aardvark, byte power_mask);
        [DllImport("aardvark")]
        private static extern int net_aa_unique_id(int aardvark);
        [DllImport("aardvark")]
        private static extern int net_aa_version(int aardvark, ref AardvarkVersion version);
        private static long tp_min(long x, long y)
        {
            if (x >= y)
            {
                return y;
            }
            return x;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AardvarkExt
        {
            public AardvarkApi.AardvarkVersion version;
            public int features;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AardvarkVersion
        {
            public short software;
            public short firmware;
            public short hardware;
            public short sw_req_by_fw;
            public short fw_req_by_sw;
            public short api_req_by_sw;
        }

        private class GCContext
        {
            private GCHandle[] handles = new GCHandle[0x10];
            private int index = 0;

            public void add(GCHandle gch)
            {
                this.handles[this.index] = gch;
                this.index++;
            }

            public void free()
            {
                while (this.index != 0)
                {
                    this.index--;
                    this.handles[this.index].Free();
                }
            }
        }
    }
}

