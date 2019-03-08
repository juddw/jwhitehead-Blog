// Challenge #1 Code - OPERATORS:
function presentAnswers() {
    // set variables
    var input1 = Number(document.getElementById("field1").value);
    var input2 = Number(document.getElementById("field2").value);
    var input3 = Number(document.getElementById("field3").value);
    var input4 = Number(document.getElementById("field4").value);
    var input5 = Number(document.getElementById("field5").value);

    // validate user input
    if (input1 == "" || input2 == "" || input3 == "" || input4 == "" || input5 == "") {
        window.alert("Please make sure to fill in all fields.")
        return;
    }

    // find the smallest number
    var smallest = Math.min(input1, input2, input3, input4, input5);
    document.getElementById("result1").innerHTML = smallest + " is the smallest number.";
    // find the largest number
    var largest = Math.max(input1, input2, input3, input4, input5);
    document.getElementById("result2").innerHTML = largest + " is the largest number.";
    // find the mean/average
    document.getElementById("result3").innerHTML = (input1 + input2 + input3 + input4 + input5) / 5 + " is the average of all numbers.";
    // find the sum
    document.getElementById("result4").innerHTML = (input1 + input2 + input3 + input4 + input5) + " is the sum of all numbers.";
    // find the product
    document.getElementById("result5").innerHTML = (input1 * input2 * input3 * input4 * input5) + " is the product of all numbers.";
}

function resetAnswers() {
    for (var i = 1; i <= 5; i++) {
        document.getElementById("result" + i).innerHTML = "";
        document.getElementById("field" + i).value = "Enter a number";
    }
}



// Challenge #2 Code - FACTORIALS:
function getAnswers() {
    // get the number entered
    var theirNum = Number(document.getElementById("field1-Factorial").value);

    // validate user input
    if (theirNum < 1) {
        window.alert("Please make sure you fill in the field with a positive number.")
        // clear the displayed answer from previous entries.
        document.getElementById("result1-Factorial").innerHTML = "";
        return;
    }

    var answer = 1;
    // loop
    for (var i = theirNum; i > 0; i--) {
        answer *= i;
    }

    document.getElementById("result1-Factorial").innerHTML = answer;
}

function resetFactorial() {
    document.getElementById("result1-Factorial").innerHTML = "";
    document.getElementById("field1-Factorial").value = "Enter a positive number.";
}



// Challenge #3 Code - FIZZBUZZ:
function testNumbers() {
    // catch the user input
    var firstNum = Number(document.getElementById("field1-FizzBuzz").value);
    var secondNum = Number(document.getElementById("field2-FizzBuzz").value);

    // validate user input
    if (firstNum > 100 || firstNum < 1 || secondNum > 100 || secondNum < 1) {
        alert("Please use a number between 1 and 100");

        // numbers are in range so begin iteration.
    } else {
        // set variable.
        var answer = "";
        for (var i = 1; i <= 100; i++) {
            var iSwap = ""; // reset to empty for next round.
            // check for even multiplier which means remainder 0.
            if (i % firstNum == 0) {
                // number being tested is a multiple of firstNum, add "Fizz"
                iSwap = "Fizz";
            }
            // check for even multiplier which means remainder 0.
            if (i % secondNum == 0) {
                // number being tested is a multiple of secondNum, add "Buzz"
                iSwap += "Buzz";
            }

            // iSwap is not empty, it found a match from above code.
            if (iSwap !== "") {
                answer += iSwap + " ";
                // iSwap has nothing in it, so just add number in sequence.
            } else {
                answer += i + " ";
            }
        }

        document.getElementById("result1-FizzBuzz").innerHTML = answer;
    }
}

function resetFizzBuzz() {
    document.getElementById("result1-FizzBuzz").innerHTML = "";
    document.getElementById("field1-FizzBuzz").value = "Enter number 1 to 100.";
    document.getElementById("field2-FizzBuzz").value = "Enter number 1 to 100.";
}



// Challenge #4 Code - Palindromes:
function testWord() {
    var theirWord = document.getElementById("field1-Palindromes").value;

    // validate user input
    if (theirWord == "") {
        window.alert("Please make sure to fill in the field.")
        return;
    }
    // look for non-alphanumeric items and replace with nothing.
    var theirWord = theirWord.replace(/[^A-Za-z0-9]/gi, "");

    // set theirWord to lowercase so it will not consider case an issue.
    // W or w I want to be treated as equal.
    theirWord = theirWord.toLowerCase();

    var wordLength = theirWord.length;
    var reversedWord = "";
    // remember JS is 0 indexed! So you need to start with length - 1.
    for (var i = wordLength - 1; i >= 0; i--) {
        reversedWord += theirWord.charAt(i);
    }

    if (reversedWord === theirWord) {
        document.getElementById("result1-Palindromes").innerHTML = "This is a palindrome!";
        // console.log("this is palindrome!")
    } else {
        document.getElementById("result1-Palindromes").innerHTML = "This is NOT a palindrome!";
        // console.log("this is not palindrome!")
    }
}

function resetPalindromes() {
    document.getElementById("result1-Palindromes").innerHTML = "";
    document.getElementById("field1-Palindromes").value = "Enter a word or phrase.";
}



/*
 Challenge #4 Code version2 No Loop - Palindromes:
function testWord() {
    var theirWord = document.getElementById("field1-Palindromes").value;
    var splitWord = theirWord.split("");
    var revWord = splitWord.reverse();
    var joinRevWord = revWord.join("");

    if (joinRevWord === theirWord) {
        document.getElementById("result1-Palindromes").innerHTML = "This is a palindrome!";
         console.log("this is palindrome!")
    } else {
        document.getElementById("result1-Palindromes").innerHTML = "This is NOT a palindrome!";
         console.log("this is not palindrome!")
    }
}
*/

// Challenge Bonus B - ABC Order:
function abcOrder() {
    // set variables
    var theirWord = document.getElementById("field1-abcOrder").value;

    // validate user input
    if (theirWord == "") {
        window.alert("Please make sure to fill in the field.")
        return;
    }

    // split word to character string
    var theirSplitWord = theirWord.split("");
    var theirWordLettersSortedAlphabetically = theirSplitWord.sort(Intl.Collator().compare);

    document.getElementById("result1-abcOrder").innerHTML = theirWordLettersSortedAlphabetically;
}

function resetABC() {
    document.getElementById("result1-abcOrder").innerHTML = "";
    document.getElementById("field1-abcOrder").value = "Enter a word.";
}



// Challenge Bonus D - Find Vowels for phrase and retun vowels and total count:
function findVowels() {
    // set variables
    var theirPhrase = document.getElementById("field1-Vowels").value;

    // validate user input
    if (theirPhrase == "") {
        window.alert("Please make sure to fill in the field.")
        return;
    }

    var justVowelsFromPhrase = theirPhrase.replace(/[^aeiouAEIOU]/g, "");
    var x = justVowelsFromPhrase.split("");
    var y = x.join(",");
    var numberOfVowels = justVowelsFromPhrase.length;

    document.getElementById("result1-Vowels").innerHTML = "List of vowels is [" + y + "] " + numberOfVowels;

}

function resetVowels() {
    document.getElementById("result1-Vowels").innerHTML = "";
    document.getElementById("field1-Vowels").value = "Enter a phrase.";
}



// Reset fields when modal is closed or clicked off of.
// Modal 1
$('#myJSModal-Math').on('hide.bs.modal', function () {
    resetAnswers();
})

// Modal 2
$('#myJSModal-Factorial').on('hide.bs.modal', function () {
    resetFactorial();
})

// Modal 3
$('#myJSModal-FizzBuzz').on('hide.bs.modal', function () {
    resetFizzBuzz();
})

// Modal 4
$('#myJSModal-Palindromes').on('hide.bs.modal', function () {
    resetPalindromes();
})

// Modal 5
$('#myJSModal-abcOrder').on('hide.bs.modal', function () {
    resetABC();
})

// Modal 6
$('#myJSModal-Vowels').on('hide.bs.modal', function () {
    resetVowels();  
})