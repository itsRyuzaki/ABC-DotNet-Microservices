# PowerShell Script to create a migration with user inputs

# Prompt user for the first input (migration name)
$input1 = Read-Host "Enter the migration name"

# Define a list of context names
$contextOptions = @("Mobiles", "Computers")

for ($i = 0; $i -lt $contextOptions.Count; $i++) {
     $command = "dotnet ef migrations add $input1 --context $($contextOptions[$i])`DataContext --output-dir Migrations\$($contextOptions[$i])"

    # Display the constructed command
    Write-Host "Generated Command: $command"

    # Execute the command
    Invoke-Expression $command
}
