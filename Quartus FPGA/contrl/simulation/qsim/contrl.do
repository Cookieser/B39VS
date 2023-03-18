onerror {quit -f}
vlib work
vlog -work work contrl.vo
vlog -work work contrl.vt
vsim -novopt -c -t 1ps -L cycloneive_ver -L altera_ver -L altera_mf_ver -L 220model_ver -L sgate work.contrl_vlg_vec_tst
vcd file -direction contrl.msim.vcd
vcd add -internal contrl_vlg_vec_tst/*
vcd add -internal contrl_vlg_vec_tst/i1/*
add wave /*
run -all
