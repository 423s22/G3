# ETD Validator v0.1 Test Plan

## Scope

THis document describes the system testing requirments for the Electronic Theses and Dissertations validator for version 0.2.

## Environment Requirements

These tests should be ran on the current [production branch of the ETD Web application](https://etdvalidator.herokuapp.com/)

## Features to be tested

This section will detail the features to be tested and the list of individual tests to verify each feature

### Document Validator

#### Document Uploader

##### Upload a completely valid [.docx file](../ETDValidatorUnitTests/TestDocuments/Completed_Example_ETD.docx)

> Test Steps:
>
> 1. Go to the [file uploader page](https://etdvalidator.herokuapp.com/LoadDocument)
> 2. Click the upload area and select a valid file with the .docx file extension
> 3. Submit the form
>
> Expected Results:
>
> - The submission is succesful
> - The page redirects to [https://etdvalidator.herokuapp.com/DocumentResults](https://etdvalidator-dev.herokuapp.com/DocumentResults)
> - The redirected page displays a message that the document
>
> Actual Results:
>
> - ![#c5f015](https://via.placeholder.com/15/c5f015/000000?text=+) `PASS`
> - Input: Uploaded the valid .docx file
> - Output: The output matched the expected results

##### Upload an invalid .docx file

> Test Steps:
>
> 1. Go to the [file uploader page](https://etdvalidator.herokuapp.com/LoadDocument)
> 2. Click the upload area and select [one of the invalid test documents](../ETDValidatorUnitTests/TestDocuments/)
> 3. Submit the form
>
> Expected Results:
>
> - The submission is succesful
> - The page redirects to [https://etdvalidator.herokuapp.com/DocumentResults](https://etdvalidator-dev.herokuapp.com/DocumentResults)
> - The redirected page succesfully displays all errors/warnings associated with the uploaded document
>
> Actual Results:
>
> - ![#c5f015](https://via.placeholder.com/15/c5f015/000000?text=+) `PASS`
> - Input: Uploaded an ivalid test .docx file
> - Output: The output matched the expected results

##### Upload a file without the .docx extension

> Test Steps:
>
> 1. Go to the [file uploader page](https://etdvalidator.herokuapp.com/LoadDocument)
> 2. Click the upload area and attempt to select a non-.docx file
> 3. If a non-.docx file can be selected, click the submit button
>
> Expected Results:
>
> - Any file without the .docx extension should not be selectable
> - If a file without the .docx extension can be selected, clicking the Validate button will cause the file upload to fail
>
> Actual Results:
>
> - ![#c5f015](https://via.placeholder.com/15/c5f015/000000?text=+) `PASS`
> - Input: Attempted to upload non-.docx file
> - Output: File upload was not possible

##### Upload a non-.docx file that has the .docx extension

> Test Steps:
>
> 1. Take a non-.docx file and change its extension to .docx
> 2. Go to the [file uploader page](https://etdvalidator.herokuapp.com/LoadDocument)
> 2. Click the upload area and upload the edited file
> 3. Submit the form
>
> Expected Results:
>
> - The file upload fails and an error message is displayed
>
> Actual Results:
>
> - ![#c5f015](https://via.placeholder.com/15/c5f015/000000?text=+) `PASS`
> - Input: Uploaded a PDF masked as a .docx file
> - Output: File upload failed and an error message was displayed
