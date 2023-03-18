onerror {resume}
quietly WaveActivateNextPane {} 0
add wave -noupdate -radix binary /contrl_tb/clk
add wave -noupdate -radix binary /contrl_tb/rst_n
add wave -noupdate -radix binary /contrl_tb/QR_in
add wave -noupdate -radix binary /contrl_tb/geo_in
add wave -noupdate -radix binary /contrl_tb/gps_in
add wave -noupdate -radix binary /contrl_tb/PWM
add wave -noupdate -radix binary /contrl_tb/RST
add wave -noupdate -radix binary /contrl_tb/DIR
add wave -noupdate -radix binary /contrl_tb/EN
add wave -noupdate -radix binary /contrl_tb/ro_motor
add wave -noupdate /contrl_tb/M_OUT
TreeUpdate [SetDefaultTree]
WaveRestoreCursors {{Cursor 1} {206268 ps} 0}
quietly wave cursor active 1
configure wave -namecolwidth 150
configure wave -valuecolwidth 100
configure wave -justifyvalue left
configure wave -signalnamewidth 0
configure wave -snapdistance 10
configure wave -datasetprefix 0
configure wave -rowmargin 4
configure wave -childrowmargin 2
configure wave -gridoffset 0
configure wave -gridperiod 1
configure wave -griddelta 40
configure wave -timeline 0
configure wave -timelineunits ps
update
WaveRestoreZoom {0 ps} {420 ns}
