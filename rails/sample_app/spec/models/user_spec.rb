# == Schema Information
#
# Table name: users
#
#  id         :integer         not null, primary key
#  name       :string(255)
#  email      :string(255)
#  created_at :datetime        not null
#  updated_at :datetime        not null
#

require 'spec_helper'

describe User do
  before { @user = User.new name: "Example", 
    email: "user@example.com",
    password: "foobar",
    password_confirmation: "foobar" }
  
  subject { @user }
  
  it { should respond_to :name }
  it { should respond_to :email }
  it { should respond_to :password_digest }
  it { should respond_to :password }
  it { should respond_to :password_confirmation }
  it { should respond_to :remember_token }
  it { should respond_to :authenticate }
  it { should respond_to :admin }
  
  it { should be_valid }
  it { should_not be_admin }

  describe "with admin attribute set to 'true'" do
    before do
      @user.save!
      @user.toggle! :admin
    end

    it { should be_admin }
  end
  
  describe "When name is not present" do
    before { @user.name = " " }
    it { should_not be_valid }
  end
  
  describe "When email is present" do
    before { @user.email = " " }
    it { should_not be_valid }
  end
  
  describe "When name is too long" do
    before { @user.name = "a" * 51 }
    it { should_not be_valid }
  end
  
  describe "When email format is valid" do
    it "should be valid" do
      addresses = %w[user@foo.COM A_US-ER@f.b.orb frst.lst@foo.jp a+b@baz.cn]
      addresses.each do |valid_address|
        @user.email = valid_address
        @user.should be_valid
      end
    end
  end
  
  describe "When email format is invalid" do
    it "should be invalid" do
      addresses = %w[user@foo,com user_at_foo.org example.user@foo. foo@bar_baz.com foo@bar+baz.com]
      addresses.each do |invalid_address|
        @user.email = invalid_address
        @user.should_not be_valid
      end 
    end
  end
  
  describe "When email address is already taken" do
    before do
      user_with_same_email = @user.dup
      user_with_same_email.email = @user.email.upcase
      user_with_same_email.save
    end
    
    it { should_not be_valid }
  end

  describe "when password is not present" do
    before do 
      @user.password = @user.password_confirmation = ""
    end

    it { should_not be_valid }
  end

  describe "when password does not match confirmation" do
    before { @user.password_confirmation = "missmatch" }
    it { should_not be_valid }
  end

  describe "when confirmation is nil" do
    before { @user.password_confirmation = nil }
    it { should_not be_valid }
  end

  describe "wit a password that is to short" do
    before do 
      @user.password = @user.password_confirmation = "a" * 5
    end
  end

  describe "return value of authenticate method" do
    before { @user.save }
    let(:found_user) { User.find_by_email(@user.email) }

    describe "with valid password" do
      it { should == found_user.authenticate(@user.password) }
    end

    describe "with invalid password" do
      let(:user_for_invalid_password) {
        found_user.authenticate "invalidpassword"
      }

      it { should_not == user_for_invalid_password }
      specify { user_for_invalid_password.should be_false }
    end
  end

  describe "when email has capital letters" do
    before do 
      @user.email = "OmarJavier@gmail.com"
      @user.save 
    end

    its(:email) { should == "omarjavier@gmail.com" }
  end

  describe "remember token" do
    before { @user.save }
    its(:remember_token) { should_not be_blank }
  end
end