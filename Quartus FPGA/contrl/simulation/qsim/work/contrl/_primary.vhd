library verilog;
use verilog.vl_types.all;
entity contrl is
    port(
        clk             : in     vl_logic;
        rst_n           : in     vl_logic;
        tem_in          : in     vl_logic_vector(7 downto 0);
        hum_in          : in     vl_logic_vector(7 downto 0);
        sun_in          : in     vl_logic_vector(7 downto 0);
        water_in        : in     vl_logic_vector(7 downto 0);
        pump            : out    vl_logic;
        tem_low         : out    vl_logic;
        tem_high        : out    vl_logic;
        hum_low         : out    vl_logic;
        sun_high        : out    vl_logic;
        water_low       : out    vl_logic
    );
end contrl;
