# ETD Validator v0.1 Test Plan

## Scope

THis document describes the system testing requirments for the Electronic Theses and Dissertations validator for version 0.1.

## Environment Requirements

These tests should be ran on the current [development branch of the ETD Web application](https://etdvalidator-dev.herokuapp.com/)

## Features to be tested

This section will detail the features to be tested and the list of individual tests to verify each feature

### Document Validator

#### Document Uploader

##### Upload a valid .docx file

> Test Steps:
>
> 1. Go to the [file uploader page](https://etdvalidator-dev.herokuapp.com/LoadDocument)
> 2. Click the upload area and select a valid file with the .docx file extension
> 3. Submit the form
>
> Expected Results:
>
> - The submission is succesful
> - The page redirects to [https://etdvalidator-dev.herokuapp.com/DocumentResults](https://etdvalidator-dev.herokuapp.com/DocumentResults)
> - The redirected page displays the validation JSON
>
> Actual Results:
>
> - ![#c5f015](https://via.placeholder.com/15/c5f015/000000?text=+) `PASS`
> - Input: Uploaded valid .docx file
> - Output: The output matched the expected results
