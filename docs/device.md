# Device
To use a user command you need to use this type of command:
```
./web-wol_cli <path of your database> user -m <your mode>
```

**⚠️ You don't have any type of validation for the input!**

## Add a device
You need to use the mode `a` to add a device.

A prompt will be show to you for the account information. You need to fullfill them.

## Delete a device
You need to use the mode `d` to delete a device.

A prompt will be show to you and asking the ID of the user to delete.

You can find the ID of the user by listing all the users in the database.

## List all the devices
You need to use the mode `l` to list all the devices.

A table will be show to you with all the important information.