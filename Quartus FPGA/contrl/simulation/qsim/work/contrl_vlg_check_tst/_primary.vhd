library verilog;
use verilog.vl_types.all;
entity contrl_vlg_check_tst is
    port(
        hum_low         : in     vl_logic;
        pump            : in     vl_logic;
        sun_high        : in     vl_logic;
        tem_high        : in     vl_logic;
        tem_low         : in     vl_logic;
        water_low       : in     vl_logic;
        sampler_rx      : in     vl_logic
    );
end contrl_vlg_check_tst;
