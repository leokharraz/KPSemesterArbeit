package utils

import (
	"bufio"
	"fmt"
	"os"
	"strings"
	"unicode"
)

func ReadInt() (int, error) {
	var value int
	for {
		_, err := fmt.Scan(&value)
		if err != nil {
			fmt.Scanln() // Clear buffer
			fmt.Print("Invalid input. Please enter a number: ")
			continue
		}
		return value, nil
	}
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
	bufio.NewScanner(os.Stdin)
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
