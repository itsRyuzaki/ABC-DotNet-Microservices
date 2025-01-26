# PowerShell Script to update database

# Define a list of context names
$contextOptions = @("Mobiles", "Computers")

for ($i = 0; $i -lt $contextOptions.Count; $i++) {
     $command = "dotnet ef database update --context $($contextOptions[$i])`DataContext"

    # Display the constructed command
    Write-Host "Generated Command: $command"

    # Execute the command
    Invoke-Expression $command
}
