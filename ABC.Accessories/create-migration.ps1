# PowerShell Script to create a migration with user inputs, including a list of options for context name

# Prompt user for the first input (migration name)
$input1 = Read-Host "Enter the migration name"

# Define a list of options for context names
$contextOptions = @("Mobiles", "Computers")

# Display the list of context options with numbers
Write-Host "Select the context name from the list:"
for ($i = 0; $i -lt $contextOptions.Count; $i++) {
    Write-Host "$($i + 1). $($contextOptions[$i])"
}

$input2Index = Read-Host "Enter the number of the context you want to choose (1-$($contextOptions.Count))"

# Validate the user's input to make sure it’s within the valid range
if ($input2Index -gt 0 -and $input2Index -le $contextOptions.Count) {
    # Get the selected context name
    $input2 = $contextOptions[$input2Index - 1]

    # Construct the command using the inputs
    $command = "dotnet ef migrations add $input1 --context $input2`DataContext --output-dir Migrations\$input2"

    # Display the constructed command
    Write-Host "Generated Command: $command"

    # Execute the command
    Invoke-Expression $command
} else {
    Write-Host "Invalid selection. Please choose a number between 1 and $($contextOptions.Count)."
}
