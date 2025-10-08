# Secret Santa

This is work in progress!

# Purpose

This project was designed for various purposes:
1. As an actual Secret Santa tool for my family (and in the future, anyone else) to use
2. To show some of my existing coding skills and knowledge
3. To try new or more up-to-date tools, packages and libraries
4. To experiment with different solution approaches, structures, patterns etc.
5. To practice creating a new solution etc. from scratch

# How To Use Secret Santa

See the CreateNewInstance.txt file.

# Decisions I Made

I've sometimes decided to try a different approach to what I'm used to, to see what happens!  

So this is experimental and not necessarily trying to use 'established best practice', what I've done elsewhere, etc.  That included using:
- Interfaces that generally extend to database tables (not sure I'd do this again, 
	as it often prevents composition over inheritance and isn't very compatible with many design patterns)
- Standard JavaScript where possible, as I was rusty (although JQuery was already embedded)
- ASP.NET Identity and User Secrets (for development) but with my own modifications
- Encryption of usernames, e-mails and security answers to keep future users safe, e.g. if this is cloned
- Base classes that extend to the view models, to try to avoid DTOs with lots of replication
- Entity Framework Core, as I'd only used EF6 before
- Bootstrap 5, as I was used to Bootstrap 3
- My own 'light' version of CQRS without using (e.g.) Mediator, to see what happens, adding 'Actions' that just update an item

I've made light use of AutoMapper, e.g. avoiding mapping into Entities, as version 12 took away a lot of functionality. 