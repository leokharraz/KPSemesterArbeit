package utils

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
	"unicode"
)

func ReadIntInRange(min, max int) (int, error) {
	scanner := bufio.NewScanner(os.Stdin)

	for scanner.Scan() {
		input := strings.TrimSpace(scanner.Text())

		value, err := strconv.Atoi(input)
		if err != nil {
			fmt.Print("Invalid input. Please enter a number: ")
			continue
		}

		// Check range
		if value < min || value > max {
			fmt.Printf("Please enter a number between %d and %d: ", min, max)
			continue
		}

		return value, nil
	}

	return 0, scanner.Err()
}

// ReadString reads a string containing only letters from standard input
// Returns the string and an error if reading fails
func ReadString() (string, error) {
	scanner := bufio.NewScanner(os.Stdin)

	for scanner.Scan() {
		input := strings.TrimSpace(scanner.Text())

		if input == "" {
			fmt.Print("Input cannot be empty. Please try again: ")
			continue
		}

		// Check if input contains only letters
		if !isLettersOnly(input) {
			fmt.Print("Name must contain only letters. Please try again: ")
			continue
		}

		return input, nil
	}

	return "", scanner.Err()
}

// isLettersOnly checks if a string contains only letters (no numbers or special characters)
func isLettersOnly(s string) bool {
	for _, char := range s {
		if !unicode.IsLetter(char) {
			return false
		}
	}
	return true
}
