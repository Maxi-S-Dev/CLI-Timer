# CLI-Timer

What is this: Basically it's a simple timer ... but with a **twist**!! 
## It is just like a CONSOLE - YAY

You don't have to touch your mouse or even click to create a Timer. Just type Text


## How to use it
--- 
### Let's start simple
In the bottom right you can find the main timer. Just type the following command
```
add 1h 30m 20s
```
Now the main timer displays this time. To start the main timer just type:
```
start
```
You made it. The timer is now running!

If you want to stop the timer just use this command
```
end
```
This will end the current timer. Why "current"?

I'll show you.

### Using Profiles
You don't have to set the time in the timer urself every time. You can simply use profiles. Profiles have predefined names, answers and lengths. You will learn later how to create your own. Some profiles are already implemented in the application. For example "work". Just type "work" in the command line.
```
work
```
Now you can see, that a 45 Minute countdown started. Also in the command history you can see, that this profile is called "work" and it's answer tells you that you just started working.

Let's say you have been really productive and you want to take a 20 minute break. Just type break
```
pause
```
The main Timer now has stopped and a second timer started running besides the answer. This is the secondary timer. If you now use the command 
```
end
```
the second timer, in this case the pause timer, will stop running and the main timer continues.

You can also override temporary how long a timer should run, by specifying the time right after the profile name.
```
work 20m
```
The work timer will now run for 20 minutes. 

### More commands
Let's explore some more commands.
```
reset
```
This command works just like "end" but it will reset all timers. (Main and secondary)

```
add <TimeToAdd>
```
We have already used add once, but you can also use it while a timer is running to extend it's time.

```
subtract <TimeToSubtract>
```
Just like add but the it will shorten the time.

```
clear
```
This will clear the command history.

```
close
```
This will close the application. (Why would you use the button in the top right)?.

### Create, Manage and Delete Profiles
I have already introduced you into profiles, but did you know, that you can create your own? Let's create one together

Write:
```
new gaming "Playing Apex Legends" 1h 30m
```
- This will create a new Profile with the name "gaming"
- Whe you start the profile, it will show "Playing Apex Legends" as the answer.
- And it will last for 1 hour and 30 Minutes.

Now that we have this profile, let's make some changes. Write
```
change gaming answer "Playing Hunt Showdown"
```
- Now instead of "Playing Apex Legends" the answer will be "Playing Hunt Showdown"

We can change everything using the following commands
```
change <ProfileName> time <New Time> //changes the timer of the profile 
change <ProfileName> name <NewName> //changes the name of the profile
change <ProfileName> answer <NewAnswer> //changes the timer of the profile

---examples---
change gaming time 1h 45m 10s
change gaming name learning
change gaming annswer "Now learning how to code"
```

In the end we might want to delete a profile. We can easily do this as follows:
```
delete gaming
```
This will delete the profile gaming.

---

## Future features
- UI-Settings Window
- Custom Gradients
- Ringtones
- Notifications
- UI Profile Management
