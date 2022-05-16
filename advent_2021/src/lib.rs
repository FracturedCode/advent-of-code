#![feature(test)]
extern crate test;

#[cfg(test)]
mod tests {
	use super::*;
	use test::Bencher;

	#[test]
	fn test_d1p1() {
		let answer = d1p1(&input::read_lines_to_int_vec("inputs/d1.txt"));
		println!("{answer}");
		assert_eq!(answer, 1521);
	}

	#[bench]
	fn bench_d1p1(b: &mut Bencher) {
		let adv_input = input::read_lines_to_int_vec("inputs/d1.txt");
		b.iter(|| d1p1(&adv_input));
	}

	#[test]
	fn test_d1p2() {
		let answer = d1p2(&input::read_lines_to_int_vec("inputs/d1.txt"));
		println!("{answer}");
		assert_eq!(answer, 1543);
	}

	#[bench]
	fn bench_d1p2(b: &mut Bencher) {
		let adv_input = input::read_lines_to_int_vec("inputs/d1.txt");
		b.iter(|| d1p2(&adv_input));
	}
}

mod input {
	use std::io::{self, BufRead};

	pub fn read_lines_to_int_vec<P>(file_path: P) -> Vec<i32>
	where P: AsRef<std::path::Path> {
		let file = std::fs::File::open(file_path).expect("Unable to open file.");
		let lines = io::BufReader::new(file).lines();
		let mut r_vec: Vec<i32> = Vec::new();
		for line in lines {
			r_vec.push(line.unwrap().parse().unwrap());
		}
		r_vec
	}
}

/// https://adventofcode.com/2021/day/1
pub fn d1p1(input: &Vec<i32>) -> i32 {
	let mut prev = i32::max_value();
	let mut sum = 0;
	for int in input {
		if *int > prev {
			sum += 1;
		}
		prev = *int;
	}
	sum
}

/// https://adventofcode.com/2021/day/1#part2
pub fn d1p2(depths: &Vec<i32>) -> i32 {
	let mut prev_sum = i32::max_value();
	let mut increase_count = 0;
	for i in 0..(depths.len()-2) {
		let sum = depths[i] + depths[i+1] + depths[i+2];
		if sum > prev_sum {
			increase_count += 1;
		}
		prev_sum = sum;
	}
	increase_count
}