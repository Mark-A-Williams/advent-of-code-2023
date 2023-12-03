#![allow(dead_code)]

extern crate stopwatch;
mod solutions;

use crate::solutions::day_8::{part_1, part_2};
use stopwatch::Stopwatch;

fn main() {
    let mut sw = Stopwatch::start_new();
    part_1();
    println!("Part 1 completed in {}ms", sw.elapsed_ms());

    sw.reset();

    sw.start();
    part_2();
    println!("Part 2 completed in {}ms", sw.elapsed_ms());
}
