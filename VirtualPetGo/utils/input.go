package utils

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
	"unicode"
)

func ReadInt() (int, error) {
	scanner := bufio.NewScanner(os.Stdin)

	for scanner.Scan() {
		input := strings.TrimSpace(scanner.Text())

		value, err := strconv.Atoi(input)
		if err != nil {
			fmt.Print("Invalid input. Please enter a number: ")
			continue
		}

		return value, nil
	}

	return 0, scanner.Err()
}

// ReadString reads a string containing only letters from standard input
func ReadString() (string, error) {
	scanner := bufio.NewScanner(os.Stdin)

	for scanner.Scan() {
		input := strings.TrimSpace(scanner.Text())

		if input == "" {
			fmt.Print("Input cannot be empty. Please try again: ")
			continue
		}

		if !isLettersOnly(input) {
			fmt.Print("Name must contain only letters. Please try again: ")
			continue
		}

		return input, nil
	}

	return "", scanner.Err()
}
func WaitForEnter() {
	fmt.Println("Press enter to continue...")
	var dummy string
	fmt.Scanln(&dummy)
}

// ReadIntInRange reads an integer within a specific range
func ReadIntInRange(min, max int) (int, error) {
	for {
		value, err := ReadInt()
		if err != nil {
			return 0, err
		}

		if value < min || value > max {
			fmt.Printf("Please enter a number between %d and %d: ", min, max)
			continue
		}

		return value, nil
	}
}

// isLettersOnly checks if a string contains only letters
func isLettersOnly(s string) bool {
	for _, char := range s {
		if !unicode.IsLetter(char) {
			return false
		}
	}
	return true
}
