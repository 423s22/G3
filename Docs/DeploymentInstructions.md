# Instructions for Production Deployment to Heroku

## Access to Heroku

- Make an account [on Heroku](https://signup.heroku.com/)
- Follow the [instructions here](https://devcenter.heroku.com/articles/heroku-cli) to download and login to the Heroku CLI
- Creat a github issue to have your account added to the production project. Once you have access, you can follow the next sesssion to deploy to it

## Pushing New Changes

From a terminal logged into the Heroku CLI:

- Navigate to the [main project directory](../SourceCode/ETDValidator/ETDValidator/) with the docker file
- Login into the Heroku container registry with `heroku container:login`
- Set your remote to the correct project with `heroku git:remote -a etdvalidator`
- Next build the dockerized application: `docker build -t etdvalidator .`
- Push the build to the web: `heroku container:push -a etdvalidator web`
- Finally, release the application: `heroku container:release -a etdvalidator web`
