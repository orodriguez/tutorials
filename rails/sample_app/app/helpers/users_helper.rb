module UsersHelper
  def gravatar_for user, ops = { size: 100 }
    gravatar_id = Digest::MD5::hexdigest user.email.downcase
    gravatar_url = "https://secure.gravatar.com/avatar/" +
      "#{gravatar_id}?s=#{ops[:size]}"
    image_tag gravatar_url, 
      atl: user.name, 
      class: "gravatar"
  end
end