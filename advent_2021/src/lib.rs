#![feature(test)]
extern crate test;

#[cfg(test)]
mod tests {
	use super::*;
	use test::Bencher;

	#[bench]
	fn bench_d1p1(b: &mut Bencher) {
		let adv_input = input::read_lines_to_int_vec(1);
		b.iter(|| d1p1(&adv_input));
		let answer = d1p1(&adv_input);
		println!("{answer}");
		assert_eq!(answer, 1521);
	}

	#[bench]
	fn bench_d1p2(b: &mut Bencher) {
		let adv_input = input::read_lines_to_int_vec(1);
		b.iter(|| d1p2(&adv_input));
		let answer = d1p2(&adv_input);
		assert_eq!(answer, 1543);
		println!("{answer}");
	}

	#[bench]
	fn bench_d2p1(b: &mut Bencher) {
		let adv_input = input::read_lines(2);
		b.iter(|| d2p1(&adv_input));
		let answer = d2p1(&adv_input);
		assert_eq!(answer, 1648020);
		println!("{answer}");
	}

	#[bench]
	fn bench_d2p2(b: &mut Bencher) {
		let adv_input = input::read_lines(2);
		b.iter(|| d2p2(&adv_input));
		let answer = d2p2(&adv_input);
		assert_eq!(answer, 1759818555);
		println!("{answer}");
	}

	#[bench]
	fn bench_d3p1(b: &mut Bencher) {
		let bytesless = input::read_file(3);
		let adv_input = bytesless.as_bytes();
		b.iter(|| d3p1(adv_input));
		let answer = d3p1(adv_input);
		assert_eq!(answer, 1458194);
		println!("{answer}");
	}
}

mod input {
	use std::io::{self, BufRead};
	use std::fs;

	const UNABLE: &str = "Unable to open file.";

	pub fn read_lines_to_int_vec(day: i8) -> Vec<i32> {
		read_lines(day).iter().map(|line| line.parse::<i32>().unwrap()).collect()
	}

	pub fn read_lines(day: i8) -> Vec<String> {
		read_lines_helper(file_name(day))
	}

	pub fn read_file(day: i8) -> String {
		fs::read_to_string(file_name(day)).expect(UNABLE)
	}

	fn file_name(day: i8) -> String {
		format!("inputs/d{}.txt", day)
	}

	fn read_lines_helper<P>(file_path: P) -> Vec<String>
	where P: AsRef<std::path::Path> {
		let file = fs::File::open(file_path).expect(UNABLE);
		let lines = io::BufReader::new(file).lines();
		lines.map(|line| line.unwrap()).collect()
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

/// https://adventofcode.com/2021/day/2
pub fn d2p1(movements: &Vec<String>) -> u32 {
	let mut horizontal: u32 = 0;
	let mut down: u32 = 0;
	let mut up: u32 = 0;
	for m in movements {
		let mut ccs = m.chars();
		let dir = match ccs.next().unwrap() {
			'f' => &mut horizontal,
			'd' => &mut down,
			'u' => &mut up,
			_ => panic!("ugg")
		};
		*dir += ccs.last().unwrap().to_digit(10).unwrap();
	}
	(down - up) * horizontal
}

/// https://adventofcode.com/2021/day/2
pub fn d2p2(movements: &Vec<String>) -> u32 {
	let mut horizontal: u32 = 0;
	let mut aim: u32 = 0;
	let mut depth: u32 = 0;
	for m in movements {
		let mut ccs = m.chars();
		let command = ccs.next().unwrap();
		let distance = ccs.last().unwrap().to_digit(10).unwrap();
		if command == 'f' {
			horizontal += distance;
			depth += distance * aim;
		} else if command == 'd' {
			aim += distance;
		} else {
			aim -= distance;
		}
	}
	horizontal * depth
}

// First try on this bad boy 😏
/// https://adventofcode.com/2021/day/3
pub fn d3p1(diagnostics: &[u8]) -> u64 {
	let mut gamma: u64 = 0;
	let lines = diagnostics.len() / 13;
	let half = lines / 2;
	for digit in 0..12 {
		let mut ones: u64 = 0;
		for line in 0..lines {
			ones += (diagnostics[line * 13 + digit] == '1' as u8) as u64;
		}
		gamma <<= 1;
		gamma = gamma | (ones > half as u64) as u64;
	}
	gamma * (!gamma & 0xFFF)
}