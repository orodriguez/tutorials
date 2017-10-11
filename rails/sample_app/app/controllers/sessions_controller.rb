class SessionsController < ApplicationController
  def new
  end

  def create
    user = User.find_by_email params[:email].downcase

    unless user && user.authenticate(params[:password])
      flash.now[:error] = 'Invalid email/password combination'
      return render 'new'
    end
    
    sign_in user
    redirect_back_or user
  end

  def destroy
    sign_out
    redirect_to root_url
  end
end