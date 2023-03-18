library verilog;
use verilog.vl_types.all;
entity contrl_vlg_sample_tst is
    port(
        clk             : in     vl_logic;
        hum_in          : in     vl_logic_vector(7 downto 0);
        rst_n           : in     vl_logic;
        sun_in          : in     vl_logic_vector(7 downto 0);
        tem_in          : in     vl_logic_vector(7 downto 0);
        water_in        : in     vl_logic_vector(7 downto 0);
        sampler_tx      : out    vl_logic
    );
end contrl_vlg_sample_tst;
