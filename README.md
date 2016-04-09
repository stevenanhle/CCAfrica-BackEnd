# CCAfrica-BackEnd
The project for a non-profit organization

For Cham and Chenyu,

This is first steps of the back-end development. I just tried to code individual function the website should have. The links between functions at this step may not clear.
Once I have design layout and when I code for the front-end part, I will integrate this part along with front-end part and the routes between functions and pages will make sense more.

Here is what I did so far and how to use the code:
FOR REGISTRATION
- I created a MySql database with just two simple tables : members and profile.
- Users need to register for an account by going to Home/Register. They fill out some information on the registration form. After they click "Submit" button, information on the registration form will be delivered and stored in the table members. Now, they are registered users. On the registration form, if they choose "Pay for membership now", they are directed to Paypal/Index. Here there two options: pay with Paypal account OR pay with credit card. 
- If they choose to pay with credit card, they need to provide card infor on the payment form. For the purpose of testing, I did not create this form. However, I will create the form later. It's simple part. 
- If they choose to pay with Paypal account, they will be directed to Paypal website for Paypal login and pay there.
- After payment is processed successfully, they will be routed to Profile page. 

FOR LOGIN:
- After users login to the website by going to Home/LogOn, they are routed to Profile page at Profile/viewProfile
- At the Profile page, they can choose to edit their profile or they can go to anywhere on the website they want.

BECOME A MEMBER 
- Users can pay membership fee at when they register for an  account or can pay later.
- When they pay for membership, information about them in database will be update with two columns: sincedate and roles.
- Columns sincedate is the date they purchase membership voucher. And column roles which will be updated with "membership" 

THINGS A MEMBER USER CAN DO BUT NON_MEMBER USER CANNOT DO
- Users whose have membership status can see all pages and all sections of the webiste including not limited to pdf file, special articles, special images, special information. 
- Users who does not membership status cannot do stuff above. 
- So far, I have tested that function and it works. 

PDF reports and files:
- I created two functions. One function allows users can see pdf file within Chrome browser before download. Another function is file download. Both of them work so far.


THINGS STILL NOT DONE YET:
- Allow one user can search and see another user's profile ( May be a little tricky task, need 3 hours to done)
- Allow admin of the webiste can do everyting he wants. This part is simple.
- 
