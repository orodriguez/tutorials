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

class User < ActiveRecord::Base
  has_secure_password
  
  attr_accessible :name, 
    :email, 
    :password, 
    :password_confirmation
  
  validates :name, 
    presence: true, 
    length: { maximum: 50 }
  
  validates :email, 
    presence: true, 
    format: { with: /\A[\w+\-.]+@[a-z\d\-.]+\.[a-z]+\z/i },
    uniqueness: { case_sensitive: false }

  validates :password,
    presence: true,
    length: { maximum: 10 }

  validates :password_confirmation,
    presence: true

  before_save :create_remember_token

  def email=(email)
    write_attribute :email, email.downcase
  end

  private
    def create_remember_token
      self.remember_token = SecureRandom.urlsafe_base64
    end
end